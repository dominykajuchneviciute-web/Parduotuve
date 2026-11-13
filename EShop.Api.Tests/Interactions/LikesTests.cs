using System.Net;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Interactions;

public class LikesTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task ToggleLike_ExistingItem_ReturnsOk()
    {
        var itemId = await AuthClient.CreateTestItemAsync();

        var likeRes = await AuthClient.PostAsync($"/api/items/{itemId}/like", null);

        Assert.Equal(HttpStatusCode.OK, likeRes.StatusCode);
    }
}
