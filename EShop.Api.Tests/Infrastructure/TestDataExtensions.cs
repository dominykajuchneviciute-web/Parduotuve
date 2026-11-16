using System.Net.Http.Json;
using System.Text.Json;
using EShop.Api.Data;
using EShop.Api.DTO;
using EShop.Api.Models;
using EShop.Api.Models.Items;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Api.Tests.Infrastructure;

public static class TestDataExtensions
{
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

    public static async Task<int> CreateOtherUserConversationAsync(this HttpClient authClient, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var item = new Clothing { Name = "Kito zmogaus preke", Condition = ItemCondition.Good, OwnerId = "other-user-999" };
        db.Items.Add(item);
        await db.SaveChangesAsync();

        var res = await authClient.PostAsync($"/api/chat/conversations?itemId={item.Id}", null);
        using var doc = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
        return doc.RootElement.GetProperty("id").GetInt32();
    }
}
