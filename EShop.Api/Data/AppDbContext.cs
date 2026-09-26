using Microsoft.EntityFrameworkCore;
using EShop.Api.Models;

namespace EShop.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();
}