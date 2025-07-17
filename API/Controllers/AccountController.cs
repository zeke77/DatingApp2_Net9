using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Data.Migrations;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace API.Controllers;

public class AccountController(ApDbContext context,ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // api/accounts/register
    public async Task<ActionResult<UserDto>> Register(
    RegisterDto registerDto)
    {
        if (await EmailExists(registerDto.Email))
        {
            return BadRequest("Email taken");
         }
        using var hmac = new HMACSHA512(); // will call dispose when goes out of scope

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        // add user to database
        context.Users.Add(user);  // ef will track changes to this object
        await context.SaveChangesAsync();

        return user.ToDto(tokenService);
    }

    // convention - if parameters are strings then api controller will only
    // look for query strings not the body of the request
    // [HttpPost("qsregister")] // api/accounts/register
    // public async Task<ActionResult<AppUser>> QSRegister(
    //    string email,
    //    string displayName,
    //    string password
    // )
    // {
       
    //     using var hmac = new HMACSHA512(); // will call dispose when goes out of scope

    //     var user = new AppUser
    //     {
    //         DisplayName = displayName,
    //         Email = email,
    //         PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
    //         PasswordSalt = hmac.Key
    //     };

    //     // add user to database
    //     context.Users.Add(user);  // ef will track changes to this object
    //     await context.SaveChangesAsync();

    //     return user;
    // }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users
                 .SingleOrDefaultAsync(
                    x => x.Email == loginDto.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email address");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid Password");
            }
        }
         return user.ToDto(tokenService);
    } 

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users
           .AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
