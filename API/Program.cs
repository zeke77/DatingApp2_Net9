using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DatingAppDb"));
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// add cors to allow client to talk to api
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// have to add header to allow cross domain
app.UseCors(x => x.AllowAnyHeader()
                 .AllowAnyMethod()
                 .WithOrigins("http://localhost:4200", "https://localhost:4200"));
                 
app.MapControllers();

app.Run();
