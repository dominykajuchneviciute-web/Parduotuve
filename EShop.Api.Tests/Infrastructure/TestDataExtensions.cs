using System.Net.Http.Json;
using EShop.Api.DTO;
using EShop.Api.Models;

namespace EShop.Api.Tests.Infrastructure;

public static class TestDataExtensions
{
    public static async Task<Item> CreateTestItemAsync(
        this HttpClient client,
        string category = "Electronics",
        string name = "Test Item")
    {
        var response = await client.PostAsJsonAsync("/api/items", new CreateItemRequest
        {
            Category = category,
            Name = name,
            Condition = ItemCondition.Good
        });

        return (await response.Content.ReadFromJsonAsync<Item>())!;
    }
}