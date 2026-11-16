using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;

namespace EShop.Api.Tests.Infrastructure;

public class RespawnDatabaseResetter : IAsyncDisposable
{
    private Respawner? _respawner;
    private DbConnection? _connection;

    public async Task InitializeAsync(IServiceProvider services)
    {
        var config = services.GetRequiredService<IConfiguration>();
        var connectionString = config.GetConnectionString("DefaultConnection")!;

        _connection = new NpgsqlConnection(connectionString);
        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public async Task ResetAsync()
    {
        if (_respawner != null && _connection != null)
        {
            await _respawner.ResetAsync(_connection);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}