using System.Net;
using System.Text.Json;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EShop.Api.Tests.Interactions;

public class SavedItemsTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task ToggleSaved_ItemAppearsInUserSavedList()
    {
        var itemId = await AuthClient.CreateTestItemAsync();

        var saveRes = await AuthClient.PostAsync($"/api/items/{itemId}/saved/toggle", null);
        Assert.Equal(HttpStatusCode.OK, saveRes.StatusCode);

        var savedListRes = await AuthClient.GetAsync("/api/items/saved");
        Assert.Equal(HttpStatusCode.OK, savedListRes.StatusCode);

        using var doc = await JsonDocument.ParseAsync(await savedListRes.Content.ReadAsStreamAsync());
        var items = doc.RootElement.EnumerateArray();
        Assert.Contains(items, item => item.GetProperty("id").GetInt32() == itemId);
    }
}