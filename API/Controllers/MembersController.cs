using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
     // localhost:5001/api/members
   
    public class MembersController(ApDbContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();
            return members;
        }

[Authorize]
        [HttpGet("{id}")]  // localhost:5001/api/members/bob-id
       public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await context.Users.FindAsync(id); // find only works with primary id

            if (member == null) return NotFound();

            return member;
        }

    }
}
