using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
}
