using System.Net;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Items;

public class DeleteItemTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task DeleteItem_WhenItemExists_ReturnsNoContent()
    {
        var itemId = await AuthClient.CreateTestItemAsync("Clothing", "Mano striuke");

        var delRes = await AuthClient.DeleteAsync($"/api/items/{itemId}");

        Assert.Equal(HttpStatusCode.NoContent, delRes.StatusCode);
    }

    [Fact]
    public async Task DeleteItem_WhenItemDoesNotExist_ReturnsNotFound()
    {
        var delRes = await AuthClient.DeleteAsync("/api/items/999999");

        Assert.Equal(HttpStatusCode.NotFound, delRes.StatusCode);
    }
}