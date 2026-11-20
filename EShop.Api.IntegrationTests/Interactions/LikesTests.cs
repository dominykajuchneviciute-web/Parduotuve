using System.Net;
using System.Text.Json;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Interactions;

public class LikesTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task ToggleLike_ExistingItem_TogglesLikeCountCorrectly()
    {
        var itemId = await AuthClient.CreateTestItemAsync();

        var likeRes = await AuthClient.PostAsync($"/api/items/{itemId}/like", null);
        Assert.Equal(HttpStatusCode.OK, likeRes.StatusCode);

        using var doc1 = await JsonDocument.ParseAsync(await likeRes.Content.ReadAsStreamAsync());
        Assert.True(doc1.RootElement.GetProperty("liked").GetBoolean());
        Assert.Equal(1, doc1.RootElement.GetProperty("totalLikes").GetInt32());

        var unlikeRes = await AuthClient.PostAsync($"/api/items/{itemId}/like", null);
        Assert.Equal(HttpStatusCode.OK, unlikeRes.StatusCode);

        using var doc2 = await JsonDocument.ParseAsync(await unlikeRes.Content.ReadAsStreamAsync());
        Assert.False(doc2.RootElement.GetProperty("liked").GetBoolean());
        Assert.Equal(0, doc2.RootElement.GetProperty("totalLikes").GetInt32());
    }
}