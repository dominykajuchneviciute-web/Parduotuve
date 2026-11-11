using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using EShop.Api.DTO;
using EShop.Api.Models;

namespace EShop.Api.Tests.Infrastructure;

public static class TestDataExtensions
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static async Task<int> CreateTestItemAsync(
        this HttpClient client,
        string category = "Clothing",
        string name = "Test Item")
    {
        var response = await client.PostAsJsonAsync("/api/items", new CreateItemRequest
        {
            Category = category,
            Name = name,
            Condition = ItemCondition.Good
        });

        response.EnsureSuccessStatusCode();

        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        return doc.RootElement.GetProperty("id").GetInt32();
    }
}