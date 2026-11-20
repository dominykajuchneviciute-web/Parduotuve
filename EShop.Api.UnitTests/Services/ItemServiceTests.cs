using EShop.Api.Models;
using EShop.Api.Models.Items;
using EShop.Api.Services;

namespace EShop.Api.UnitTests.Services;

public class ItemServiceTests
{
    [Fact]
    public async Task AddGetAndDelete_ManagesItemsCorrectly()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var service = new ItemService(context);

        var item = await service.AddAsync(new Clothing { Name = "Kelnės", OwnerId = "user-1", Condition = ItemCondition.New });
        Assert.Single(await service.GetAllAsync());
        Assert.Single(await service.GetByOwnerAsync("user-1"));
        Assert.NotNull(await service.GetByIdAsync(item.Id));

        Assert.False(await service.DeleteAsync(item.Id, "wrong-user"));
        Assert.True(await service.DeleteAsync(item.Id, "user-1"));
        Assert.Empty(await service.GetAllAsync());
    }

    [Fact]
    public async Task GetFilteredAsync_FiltersByTypeAndCondition()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var service = new ItemService(context);

        await service.AddRangeAsync(new List<Item>
        {
            new Clothing { Name = "Striuke", Condition = ItemCondition.New, Color = "Juoda" },
            new Electronics { Name = "Telefonas", Condition = ItemCondition.Good }
        });

        var filtered = await service.GetFilteredAsync(new ItemFilter
        {
            ItemType = "clothing",
            Condition = ItemCondition.New
        });

        Assert.Single(filtered);
        Assert.Equal("Striuke", filtered[0].Name);
    }

    [Fact]
    public async Task DeleteAsModeratorAsync_RemovesItem()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var service = new ItemService(context);

        var item = await service.AddAsync(new Clothing { Name = "Blogas skelbimas", OwnerId = "user-1" });
        var deleted = await service.DeleteAsModeratorAsync(item.Id);

        Assert.True(deleted);
        Assert.Null(await service.GetByIdAsync(item.Id));
    }
}