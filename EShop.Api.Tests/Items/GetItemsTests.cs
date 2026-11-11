using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EShop.Api.Tests.Items;

public class GetItemsTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetItems_WhenDatabaseEmpty_ReturnsOkWithEmptyList()
    {
        var response = await Client.GetAsync("/api/items");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
        Assert.Equal(0, doc.RootElement.GetArrayLength());
    }

    [Fact]
    public async Task GetItem_NonExistingId_ReturnsNotFound()
    {
        var response = await Client.GetAsync("/api/items/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}