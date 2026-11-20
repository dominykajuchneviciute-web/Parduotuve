using System.Net;
using System.Net.Http.Json;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Auth;

public class AuthApiTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Register_ValidCredentials_ReturnsOk()
    {
        var regRes = await Client.PostAsJsonAsync("/api/auth/register", new
        {
            email = "naujas@eshop.lt",
            password = "Password123!"
        });

        Assert.Equal(HttpStatusCode.OK, regRes.StatusCode);
    }

    [Fact]
    public async Task Login_RegisteredUser_ReturnsOk()
    {
        var email = "prisijungimas@eshop.lt";
        var password = "Password123!";
        await Client.PostAsJsonAsync("/api/auth/register", new { email, password });

        var loginRes = await Client.PostAsJsonAsync("/api/auth/login?useCookies=true", new { email, password });

        Assert.Equal(HttpStatusCode.OK, loginRes.StatusCode);
    }

    [Fact]
    public async Task Logout_AuthenticatedUser_ReturnsOk()
    {
        var logoutRes = await AuthClient.PostAsync("/api/auth/logout", null);

        Assert.Equal(HttpStatusCode.OK, logoutRes.StatusCode);
    }
}