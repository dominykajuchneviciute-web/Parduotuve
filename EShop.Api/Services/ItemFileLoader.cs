using System.Text.Json;
using EShop.Api.Models;

namespace EShop.Api.Services;

public class ItemFileLoader
{
    public async Task<List<Item>> LoadAsync(Stream stream)
    {
        var items = await JsonSerializer.DeserializeAsync<List<Item>>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return items ?? new List<Item>();
    }
}