using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EShop.Api.DTO;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EShop.Api.Tests.Items;

public class CreateItemSuccessTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateItem_ValidData_ReturnsCreatedAndAppearsInGetItems()
    {
        var createdId = await AuthClient.CreateTestItemAsync("Clothing", "Pavasarinis svarkas");

        var getResponse = await Client.GetAsync("/api/items");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        using var doc = await JsonDocument.ParseAsync(await getResponse.Content.ReadAsStreamAsync());
        var array = doc.RootElement.EnumerateArray();

        Assert.Contains(array, element =>
            element.GetProperty("id").GetInt32() == createdId &&
            element.GetProperty("name").GetString() == "Pavasarinis svarkas");
    }

    [Theory]
    [InlineData("Clothing", "Megztinis")]
    [InlineData("Electronics", "Ausines")]
    [InlineData("Furniture", "Kede")]
    public async Task CreateItem_SupportedCategories_ReturnsCreated(string category, string name)
    {
        var response = await AuthClient.PostAsJsonAsync("/api/items", new CreateItemRequest { Category = category, Name = name });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}