using System.Net;
using System.Net.Http.Json;
using EShop.Api.DTO;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EShop.Api.Tests.Items;

public class CreateItemAuthTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateItem_WithoutAuth_ReturnsUnauthorized()
    {
        var request = new CreateItemRequest { Category = "Clothing", Name = "Striuke" };

        var response = await Client.PostAsJsonAsync("/api/items", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}