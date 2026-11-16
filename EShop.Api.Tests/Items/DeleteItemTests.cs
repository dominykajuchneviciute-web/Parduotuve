using System.Net;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Items;

public class DeleteItemTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task DeleteItem_ExistingAndNonExisting_ReturnsExpectedStatus()
    {
        var itemId = await AuthClient.CreateTestItemAsync("Clothing", "Mano striuke");

        var delRes = await AuthClient.DeleteAsync($"/api/items/{itemId}");
        Assert.Equal(HttpStatusCode.NoContent, delRes.StatusCode);

        var delAgainRes = await AuthClient.DeleteAsync($"/api/items/{itemId}");
        Assert.Equal(HttpStatusCode.NotFound, delAgainRes.StatusCode);
    }
}
