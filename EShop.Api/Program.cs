using EShop.Api.Data;
using EShop.Api.Models;
using Microsoft.EntityFrameworkCore;
using EShop.Api.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ItemFileLoader>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("ReactApp");
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var auth = app.MapGroup("/api/auth");
auth.MapIdentityApi<IdentityUser>();
auth.MapPost("/logout", async (SignInManager<IdentityUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization();

app.MapControllers();
app.Run();

//public record ItemDto(int Id, string Name, string Description, string? Condition, string? Size, string? Manufacturer, string? Color);
//public record CreateItemRequest(string Name, string Description, string? Condition, string? Size, string? Manufacturer, string? Color);