
using System.Text.Json;
using System.Text.Json.Serialization;
using EShop.Api.Models;

namespace EShop.Api.Services;

public class ItemFileLoader
{
    public async Task<List<Item>> LoadAsync(Stream stream)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        var items = await JsonSerializer.DeserializeAsync<List<Item>>(
            stream,
            options);

        return items ?? new List<Item>();
    }
}
