using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EShop.Api.Models;
using EShop.Api.Models.Items;

namespace EShop.Api.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Item>()
            .HasDiscriminator<string>("ItemType")
            .HasValue<Clothing>("Clothing")
            .HasValue<Electronics>("Electronics")
            .HasValue<Furniture>("Furniture")
            .HasValue<Transport>("Transport")
            .HasValue<Footwear>("Footwear")
            .HasValue<Instrument>("Instrument");
    }
}