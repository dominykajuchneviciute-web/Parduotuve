using EShop.Api.Generics;
using EShop.Api.Models;
using EShop.Api.Models.Items;
using EShop.Api.Services;

namespace EShop.Api.UnitTests.Services;

public class ItemServiceTests
{
    [Fact]
    public async Task AddAndGetById_ValidItem_ReturnsSavedItem()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var service = new ItemService(context, new ItemFilterRegistry<Item>());

        var item = new Clothing { Name = "Striukė", OwnerId = "user-1", Condition = ItemCondition.New };
        var saved = await service.AddAsync(item);

        var fetched = await service.GetByIdAsync(saved.Id);
        Assert.NotNull(fetched);
        Assert.Equal("Striukė", fetched.Name);
    }

    [Fact]
    public async Task GetByOwner_MultipleUsers_ReturnsOnlyOwnersItems()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var service = new ItemService(context, new ItemFilterRegistry<Item>());

        await service.AddAsync(new Clothing { Name = "Prekė 1", OwnerId = "user-1" });
        await service.AddAsync(new Clothing { Name = "Prekė 2", OwnerId = "user-2" });

        var user1Items = await service.GetByOwnerAsync("user-1");

        Assert.Single(user1Items);
        Assert.Equal("Prekė 1", user1Items[0].Name);
    }

    [Fact]
    public async Task Delete_WrongOwner_ReturnsFalseAndKeepsItem()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var service = new ItemService(context, new ItemFilterRegistry<Item>());

        var saved = await service.AddAsync(new Clothing { Name = "Mano prekė", OwnerId = "owner-1" });

        var deleted = await service.DeleteAsync(saved.Id, "hacker-2");

        Assert.False(deleted);
        Assert.NotNull(await service.GetByIdAsync(saved.Id));
    }

    [Fact]
    public async Task Delete_CorrectOwner_ReturnsTrueAndRemovesItem()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var service = new ItemService(context, new ItemFilterRegistry<Item>());

        var saved = await service.AddAsync(new Clothing { Name = "Mano prekė", OwnerId = "owner-1" });

        var deleted = await service.DeleteAsync(saved.Id, "owner-1");

        Assert.True(deleted);
        Assert.Null(await service.GetByIdAsync(saved.Id));
    }

    [Fact]
    public async Task DeleteAsModerator_ItemWithCommentsAndLikes_RemovesAllRelatedData()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var item = new Clothing { Name = "Pažeidžianti prekė", OwnerId = "user-1" };
        context.Items.Add(item);
        await context.SaveChangesAsync();

        context.ItemComments.Add(new ItemComment { ItemId = item.Id, UserId = "user-2", Text = "Komentaras" });
        context.ItemLikes.Add(new ItemLike { ItemId = item.Id, UserId = "user-2" });
        await context.SaveChangesAsync();

        var service = new ItemService(context, new ItemFilterRegistry<Item>());
        var result = await service.DeleteAsModeratorAsync(item.Id);

        Assert.True(result);
        Assert.Empty(context.Items);
        Assert.Empty(context.ItemComments);
        Assert.Empty(context.ItemLikes);
    }
}