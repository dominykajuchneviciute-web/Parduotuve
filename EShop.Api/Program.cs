using Microsoft.EntityFrameworkCore;
using EShop.Api.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.MapGet("/api/status", () => new { Message = "Sveiki! Parduotuves backend serveris veikia sekmingai." });

app.MapControllers();

app.Run();