using EShop.Api.Generics;
using EShop.Api.Models;
using EShop.Api.Models.Items;

namespace EShop.Api.Tests.Unit;

public class ItemFilterRegistryTests
{
    [Fact]
    public void Apply_RegisteredCategory_FiltersOnlyMatchingTypeAndCondition()
    {
        var registry = new ItemFilterRegistry<Item>();
        registry.Register<Clothing>("clothing", (query, _) => query.Where(c => c.Color == "Juoda"));

        var items = new List<Item>
        {
            new Clothing { Name = "Juoda striuke", Color = "Juoda" },
            new Clothing { Name = "Balta striuke", Color = "Balta" },
            new Electronics { Name = "Telefonas", Color = "Juoda" }
        }.AsQueryable();

        var result = registry.Apply(items, "clothing", new ItemFilter()).ToList();

        Assert.Single(result);
        Assert.Equal("Juoda striuke", result[0].Name);
    }

    [Fact]
    public void Apply_NullOrEmptyItemType_ReturnsUnfilteredQuery()
    {
        var registry = new ItemFilterRegistry<Item>();
        var items = new List<Item>
        {
            new Clothing { Name = "Preke 1" },
            new Electronics { Name = "Preke 2" }
        }.AsQueryable();

        var result = registry.Apply(items, null, new ItemFilter()).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Apply_UnregisteredCategory_ThrowsArgumentException()
    {
        var registry = new ItemFilterRegistry<Item>();
        var items = new List<Item>().AsQueryable();

        Assert.Throws<ArgumentException>(() => registry.Apply(items, "nezinomas", new ItemFilter()));
    }
}