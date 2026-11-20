using EShop.Api.Hubs;
using EShop.Api.Models.Items;
using EShop.Api.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace EShop.Api.UnitTests.Services;

public class ChatServiceTests
{
    private static Mock<IHubContext<ChatHub>> CreateHubMock()
    {
        var clientProxyMock = new Mock<IClientProxy>();
        var clientsMock = new Mock<IHubClients>();
        clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(clientProxyMock.Object);

        var hubMock = new Mock<IHubContext<ChatHub>>();
        hubMock.Setup(h => h.Clients).Returns(clientsMock.Object);
        return hubMock;
    }

    [Fact]
    public async Task CreateConversationAsync_ValidItem_CreatesConversation()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Striuke", OwnerId = "owner-1" });
        await context.SaveChangesAsync();

        var service = new ChatService(context, CreateHubMock().Object);

        var result = await service.CreateConversationAsync(1, "buyer-1");

        Assert.False(result.IsNotFound);
        Assert.False(result.IsBadRequest);
        Assert.True(result.Value > 0);
    }

    [Fact]
    public async Task CreateConversationAsync_SelfChat_ReturnsBadRequest()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Striuke", OwnerId = "user-1" });
        await context.SaveChangesAsync();

        var service = new ChatService(context, CreateHubMock().Object);

        var result = await service.CreateConversationAsync(1, "user-1");

        Assert.True(result.IsBadRequest);
        Assert.Equal("Negalite pradėti pokalbio su savimi.", result.Error);
    }

    [Fact]
    public async Task SendMessageAsync_ValidMessage_SavesAndReturnsMessageDto()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Striuke", OwnerId = "owner-1" });
        await context.SaveChangesAsync();

        var service = new ChatService(context, CreateHubMock().Object);
        var conv = await service.CreateConversationAsync(1, "buyer-1");

        var msgResult = await service.SendMessageAsync(conv.Value, "buyer-1", "Labas!");

        Assert.NotNull(msgResult.Value);
        Assert.Equal("Labas!", msgResult.Value.Text);
        Assert.True(msgResult.Value.IsMine);
    }

    [Fact]
    public async Task GetMessagesAsync_ExistingMessages_ReturnsMessagesWithCorrectOwnership()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Striuke", OwnerId = "owner-1" });
        await context.SaveChangesAsync();

        var service = new ChatService(context, CreateHubMock().Object);
        var conv = await service.CreateConversationAsync(1, "buyer-1");
        await service.SendMessageAsync(conv.Value, "buyer-1", "Labas!");

        var messages = await service.GetMessagesAsync(conv.Value, "owner-1");

        Assert.Single(messages.Value!);
        Assert.False(messages.Value![0].IsMine);
    }

    [Fact]
    public async Task DeleteConversationAsync_ExistingConversation_HidesConversationForUser()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Striuke", OwnerId = "owner-1" });
        await context.SaveChangesAsync();

        var service = new ChatService(context, CreateHubMock().Object);
        var conv = await service.CreateConversationAsync(1, "buyer-1");

        var deleted = await service.DeleteConversationAsync(conv.Value, "buyer-1");

        Assert.True(deleted.Value);
        Assert.Empty(await service.GetConversationsAsync("buyer-1"));
    }
}