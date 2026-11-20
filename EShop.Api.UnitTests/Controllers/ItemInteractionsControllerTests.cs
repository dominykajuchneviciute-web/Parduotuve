using EShop.Api.Controllers;
using EShop.Api.DTO;
using EShop.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.UnitTests.Controllers;

public class ItemInteractionsControllerTests
{
    [Fact]
    public async Task AddComment_EmptyText_ReturnsBadRequest()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var controller = new ItemInteractionsController(new CommentLikeService(context))
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-1")
        };

        var result = await controller.AddComment(1, new CreateCommentRequest(""));

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ToggleLike_ValidUser_ReturnsOk()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var controller = new ItemInteractionsController(new CommentLikeService(context))
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-1")
        };

        var result = await controller.ToggleLike(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteComment_AsModerator_ReturnsNoContent()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var service = new CommentLikeService(context);
        var comment = await service.AddCommentAsync(1, "author-1", "Komentaras");

        var controller = new ItemInteractionsController(service)
        {
            ControllerContext = TestHelpers.CreateControllerContext("mod-1", "Moderator")
        };

        var result = await controller.DeleteComment(1, comment.Id);

        Assert.IsType<NoContentResult>(result);
    }
}