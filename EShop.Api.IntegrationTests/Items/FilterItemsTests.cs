using System.Net;
using System.Text.Json;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Items;

public class FilterItemsTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetItems_WithNameFilter_ReturnsOnlyMatchingItem()
    {
        await AuthClient.CreateTestItemAsync("Clothing", "Megztinis");
        await AuthClient.CreateTestItemAsync("Electronics", "Kompiuteris");

        var response = await Client.GetAsync("/api/items?Name=Megztinis");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var items = doc.RootElement.EnumerateArray().ToList();
        Assert.Single(items);
        Assert.Equal("Megztinis", items[0].GetProperty("name").GetString());
    }

    [Theory]
    [InlineData("?ItemType=Clothing&Color=Juoda&Manufacturer=Nike&Size=42")]
    [InlineData("?ItemType=Electronics&Manufacturer=Sony&Model=WH&Color=Juoda")]
    [InlineData("?ItemType=Furniture&Material=Medis&Color=Ruda&Dimensions=100x50")]
    [InlineData("?ItemType=Transport&Manufacturer=BMW&Model=X5&Year=2020")]
    [InlineData("?ItemType=Footwear&ShoeSize=43&Color=Balta&Manufacturer=Adidas")]
    [InlineData("?ItemType=Instrument&InstrumentType=Gitara&Manufacturer=Fender&Year=2019")]
    public async Task GetItems_WithCategoryFilters_ReturnsOkAndEmptyWhenNoMatch(string queryString)
    {
        await AuthClient.CreateTestItemAsync("Clothing", "Paprastas rubas");

        var response = await Client.GetAsync($"/api/items{queryString}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(0, doc.RootElement.GetArrayLength());
    }

    [Fact]
    public async Task GetItems_WithInvalidItemType_ReturnsBadRequest()
    {
        var response = await Client.GetAsync("/api/items?ItemType=InvalidType");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}