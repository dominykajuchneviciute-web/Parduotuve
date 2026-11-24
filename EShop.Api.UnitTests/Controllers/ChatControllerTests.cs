using EShop.Api.Controllers;
using EShop.Api.DTOs.Chat;
using EShop.Api.Services;
using EShop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EShop.Api.UnitTests.Controllers;

public class ChatControllerTests
{
    [Fact]
    public async Task CreateConversation_ItemNotFound_ReturnsNotFound()
    {
        var mockService = new Mock<IChatService>();
        mockService
            .Setup(s => s.CreateConversationAsync(999, "buyer-1"))
            .ReturnsAsync(ChatResult<int>.NotFound("Prekė nerasta."));

        var controller = new ChatController(mockService.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("buyer-1")
        };

        var result = await controller.CreateConversation(999);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task SendMessage_EmptyText_ReturnsBadRequest()
    {
        var mockService = new Mock<IChatService>();
        mockService
            .Setup(s => s.SendMessageAsync(1, "buyer-1", "   "))
            .ReturnsAsync(ChatResult<MessageDto>.BadRequest("Žinutė negali būti tuščia."));

        var controller = new ChatController(mockService.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("buyer-1")
        };

        var result = await controller.SendMessage(1, new SendMessageRequest { Text = "   " });

        Assert.IsType<BadRequestObjectResult>(result);
    }
}