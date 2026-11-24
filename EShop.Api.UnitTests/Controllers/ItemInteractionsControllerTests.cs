using EShop.Api.Controllers;
using EShop.Api.DTO;
using EShop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EShop.Api.UnitTests.Controllers;

public class ItemInteractionsControllerTests
{
    [Fact]
    public async Task AddComment_EmptyText_ReturnsBadRequest()
    {
        var mockService = new Mock<ICommentLikeService>();
        var controller = new ItemInteractionsController(mockService.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-1")
        };

        var result = await controller.AddComment(1, new CreateCommentRequest(""));

        Assert.IsType<BadRequestObjectResult>(result);
        mockService.Verify(
            s => s.AddCommentAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task ToggleLike_ValidUser_ReturnsOk()
    {
        var mockService = new Mock<ICommentLikeService>();
        mockService.Setup(s => s.ToggleLikeAsync(1, "user-1")).ReturnsAsync(true);
        mockService.Setup(s => s.GetLikesCountAsync(1)).ReturnsAsync(1);

        var controller = new ItemInteractionsController(mockService.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-1")
        };

        var result = await controller.ToggleLike(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteComment_AsModerator_ReturnsNoContent()
    {
        var mockService = new Mock<ICommentLikeService>();
        mockService.Setup(s => s.DeleteCommentAsModeratorAsync(10)).ReturnsAsync(true);

        var controller = new ItemInteractionsController(mockService.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("mod-1", "Moderator")
        };

        var result = await controller.DeleteComment(1, 10);

        Assert.IsType<NoContentResult>(result);
        mockService.Verify(s => s.DeleteCommentAsModeratorAsync(10), Times.Once);
    }
}