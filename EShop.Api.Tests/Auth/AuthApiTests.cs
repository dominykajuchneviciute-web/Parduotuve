using System.Net;
using System.Net.Http.Json;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Auth;

public class AuthApiTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Register_Login_And_Logout_WorksCorrectly()
    {
        var email = "naujas@eshop.lt";
        var password = "Password123!";

        var regRes = await Client.PostAsJsonAsync("/api/auth/register", new { email, password });
        Assert.Equal(HttpStatusCode.OK, regRes.StatusCode);

        var loginRes = await Client.PostAsJsonAsync("/api/auth/login?useCookies=true", new { email, password });
        Assert.Equal(HttpStatusCode.OK, loginRes.StatusCode);

        var logoutRes = await AuthClient.PostAsync("/api/auth/logout", null);
        Assert.Equal(HttpStatusCode.OK, logoutRes.StatusCode);
    }
}
