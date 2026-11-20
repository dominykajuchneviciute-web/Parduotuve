using EShop.Api.Models.Items;
using EShop.Api.Services;

namespace EShop.Api.UnitTests.Services;

public class CommentLikeServiceTests
{
    [Fact]
    public async Task Comments_AddGetAndDelete_WorksCorrectly()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var service = new CommentLikeService(context);

        var added = await service.AddCommentAsync(10, "user-1", "Puiki preke");
        var comments = await service.GetCommentsByItemAsync(10);

        Assert.Single(comments);
        Assert.Equal("Puiki preke", comments[0].Text);

        var wrongUserDelete = await service.DeleteCommentAsync(added.Id, "other-user");
        Assert.False(wrongUserDelete);

        var ownerDelete = await service.DeleteCommentAsync(added.Id, "user-1");
        Assert.True(ownerDelete);
        Assert.Empty(await service.GetCommentsByItemAsync(10));
    }

    [Fact]
    public async Task LikesAndSaved_ToggleWorksCorrectly()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 5, Name = "Batai", OwnerId = "owner-1" });
        await context.SaveChangesAsync();

        var service = new CommentLikeService(context);

        Assert.True(await service.ToggleLikeAsync(5, "user-1"));
        Assert.Equal(1, await service.GetLikesCountAsync(5));
        Assert.False(await service.ToggleLikeAsync(5, "user-1"));
        Assert.Equal(0, await service.GetLikesCountAsync(5));

        Assert.True(await service.ToggleSavedAsync(5, "user-1"));
        Assert.True(await service.IsItemSavedAsync(5, "user-1"));
        Assert.Single(await service.GetUserSavedItemsAsync("user-1"));
    }
}