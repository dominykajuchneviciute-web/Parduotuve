using System.Net;
using System.Text.Json;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Interactions;

public class LikesTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task ToggleLike_WhenNotLiked_IncreasesLikeCountToOne()
    {
        var itemId = await AuthClient.CreateTestItemAsync();

        var likeRes = await AuthClient.PostAsync($"/api/items/{itemId}/like", null);

        Assert.Equal(HttpStatusCode.OK, likeRes.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await likeRes.Content.ReadAsStreamAsync());
        Assert.True(doc.RootElement.GetProperty("liked").GetBoolean());
        Assert.Equal(1, doc.RootElement.GetProperty("totalLikes").GetInt32());
    }

    [Fact]
    public async Task ToggleLike_WhenAlreadyLiked_DecreasesLikeCountToZero()
    {
        var itemId = await AuthClient.CreateTestItemAsync();
        await AuthClient.PostAsync($"/api/items/{itemId}/like", null);

        var unlikeRes = await AuthClient.PostAsync($"/api/items/{itemId}/like", null);

        Assert.Equal(HttpStatusCode.OK, unlikeRes.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await unlikeRes.Content.ReadAsStreamAsync());
        Assert.False(doc.RootElement.GetProperty("liked").GetBoolean());
        Assert.Equal(0, doc.RootElement.GetProperty("totalLikes").GetInt32());
    }
}