using EShop.Api.Controllers;
using EShop.Api.DTOs.Chat;
using EShop.Api.Hubs;
using EShop.Api.Models.Items;
using EShop.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace EShop.Api.UnitTests.Controllers;

public class ChatControllerTests
{
    [Fact]
    public async Task CreateConversation_ItemNotFound_ReturnsNotFound()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var hubMock = new Mock<IHubContext<ChatHub>>();
        var controller = new ChatController(new ChatService(context, hubMock.Object))
        {
            ControllerContext = TestHelpers.CreateControllerContext("buyer-1")
        };

        var result = await controller.CreateConversation(999);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task SendMessage_EmptyText_ReturnsBadRequest()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Preke", OwnerId = "owner-1" });
        await context.SaveChangesAsync();

        var hubMock = new Mock<IHubContext<ChatHub>>();
        var controller = new ChatController(new ChatService(context, hubMock.Object))
        {
            ControllerContext = TestHelpers.CreateControllerContext("buyer-1")
        };

        var result = await controller.SendMessage(1, new SendMessageRequest { Text = "   " });

        Assert.IsType<BadRequestObjectResult>(result);
    }
}