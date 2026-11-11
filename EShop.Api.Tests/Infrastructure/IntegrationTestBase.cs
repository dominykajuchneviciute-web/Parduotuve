using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EShop.Api.Tests.Infrastructure;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;
    protected readonly HttpClient AuthClient;
    private readonly RespawnDatabaseResetter _dbResetter = new();

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
        AuthClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.AuthenticationScheme, _ => { });
            });
        }).CreateClient();
    }

    public async Task InitializeAsync()
    {
        using var scope = Factory.Services.CreateScope();
        await _dbResetter.InitializeAsync(scope.ServiceProvider);
        await _dbResetter.ResetAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbResetter.DisposeAsync();
    }
}