using System.Net;
using System.Text.Json;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Interactions;

public class SavedItemsTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task ToggleSaved_WhenNotSaved_MarksItemAsSaved()
    {
        var itemId = await AuthClient.CreateTestItemAsync();

        var saveRes = await AuthClient.PostAsync($"/api/items/{itemId}/saved/toggle", null);

        Assert.Equal(HttpStatusCode.OK, saveRes.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await saveRes.Content.ReadAsStreamAsync());
        Assert.True(doc.RootElement.GetProperty("isSaved").GetBoolean());
    }

    [Fact]
    public async Task ToggleSaved_WhenAlreadySaved_UnmarksItemAsSaved()
    {
        var itemId = await AuthClient.CreateTestItemAsync();
        await AuthClient.PostAsync($"/api/items/{itemId}/saved/toggle", null);

        var unsaveRes = await AuthClient.PostAsync($"/api/items/{itemId}/saved/toggle", null);

        Assert.Equal(HttpStatusCode.OK, unsaveRes.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await unsaveRes.Content.ReadAsStreamAsync());
        Assert.False(doc.RootElement.GetProperty("isSaved").GetBoolean());
    }

    [Fact]
    public async Task GetSavedItems_AfterSavingItem_ReturnsSavedItemInList()
    {
        var itemId = await AuthClient.CreateTestItemAsync();
        await AuthClient.PostAsync($"/api/items/{itemId}/saved/toggle", null);

        var savedListRes = await AuthClient.GetAsync("/api/items/saved");

        Assert.Equal(HttpStatusCode.OK, savedListRes.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await savedListRes.Content.ReadAsStreamAsync());
        var items = doc.RootElement.EnumerateArray();
        Assert.Contains(items, item => item.GetProperty("id").GetInt32() == itemId);
    }
}