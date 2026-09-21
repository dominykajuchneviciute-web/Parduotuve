using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EShop.Api.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Surandame appsettings.json failą
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Konfigūruojame PostgreSQL
        builder.UseNpgsql(connectionString);

        return new AppDbContext(builder.Options);
    }
}