using System.Text;
using System.Text.Json;
using EShop.Api.Models.Items;
using EShop.Api.Services;

namespace EShop.Api.UnitTests.Services;

public class ItemFileLoaderTests
{
    [Fact]
    public async Task LoadAsync_ValidPolymorphicJson_ReturnsTypedItems()
    {
        var json = """
        [
            { "itemType": "Clothing", "name": "Striuke", "condition": 0, "color": "Juoda" },
            { "itemType": "Electronics", "name": "Ausines", "condition": 1, "manufacturer": "Sony" }
        ]
        """;
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var loader = new ItemFileLoader();

        var items = await loader.LoadAsync(stream);

        Assert.Equal(2, items.Count);
        Assert.IsType<Clothing>(items[0]);
        Assert.IsType<Electronics>(items[1]);
        Assert.Equal("Striuke", items[0].Name);
        Assert.Equal("Ausines", items[1].Name);
    }

    [Fact]
    public async Task LoadAsync_NullJsonLiteral_ReturnsEmptyList()
    {
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes("null"));
        var loader = new ItemFileLoader();

        var items = await loader.LoadAsync(stream);

        Assert.NotNull(items);
        Assert.Empty(items);
    }

    [Fact]
    public async Task LoadAsync_EmptyArray_ReturnsEmptyList()
    {
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes("[]"));
        var loader = new ItemFileLoader();

        var items = await loader.LoadAsync(stream);

        Assert.Empty(items);
    }

    [Fact]
    public async Task LoadAsync_InvalidJson_ThrowsJsonException()
    {
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes("{ blogas json"));
        var loader = new ItemFileLoader();

        await Assert.ThrowsAsync<JsonException>(() => loader.LoadAsync(stream));
    }
}