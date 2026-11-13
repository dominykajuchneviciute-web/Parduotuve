using System.Net;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Items;

public class FilterItemsTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Theory]
    [InlineData("?Name=Megztinis")]
    [InlineData("?ItemType=Clothing&Color=Juoda&Manufacturer=Nike&Size=42")]
    [InlineData("?ItemType=Electronics&Manufacturer=Sony&Model=WH&Color=Juoda")]
    [InlineData("?ItemType=Furniture&Material=Medis&Color=Ruda&Dimensions=100x50")]
    [InlineData("?ItemType=Transport&Manufacturer=BMW&Model=X5&Year=2020")]
    [InlineData("?ItemType=Footwear&ShoeSize=43&Color=Balta&Manufacturer=Adidas")]
    [InlineData("?ItemType=Instrument&InstrumentType=Gitara&Manufacturer=Fender&Year=2019")]
    public async Task GetItems_WithFilters_ReturnsOk(string queryString)
    {
        await AuthClient.CreateTestItemAsync("Clothing", "Megztinis");

        var response = await Client.GetAsync($"/api/items{queryString}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetItems_WithInvalidItemType_ReturnsBadRequest()
    {
        var response = await Client.GetAsync("/api/items?ItemType=InvalidType");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
