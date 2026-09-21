using Microsoft.EntityFrameworkCore;
using EShop.Api.Models;

namespace EShop.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> users => Set<User>();
    public DbSet<Item> items => Set<Item>();
    public DbSet<Category> categories => Set<Category>();
    public DbSet<Comment> comments => Set<Comment>();
    public DbSet<Like> likes => Set<Like>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Užtikriname, kad tas pats vartotojas tam pačiam daiktui negalėtų uždėti kelių Like
        modelBuilder.Entity<Like>()
            .HasIndex(l => new { l.userId, l.itemId })
            .IsUnique();
    }
}