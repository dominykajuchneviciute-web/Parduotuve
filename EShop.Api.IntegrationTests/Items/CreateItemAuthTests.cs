using System.Net;
using System.Net.Http.Json;
using EShop.Api.DTO;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Items;

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
