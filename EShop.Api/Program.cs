using EShop.Api.Data;
using EShop.Api.Models;
using Microsoft.EntityFrameworkCore;
using EShop.Api.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ItemFileLoader>();

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
app.UseHsts();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

//public record ItemDto(int Id, string Name, string Description, string? Condition, string? Size, string? Manufacturer, string? Color);
//public record CreateItemRequest(string Name, string Description, string? Condition, string? Size, string? Manufacturer, string? Color);