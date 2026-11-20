using System.Security.Claims;
using EShop.Api.Hubs;
using EShop.Api.Models.Chat;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace EShop.Api.UnitTests.Hubs;

public class ChatHubTests
{
    private static (ChatHub hub, Mock<IGroupManager> groupsMock) CreateChatHub(
        EShop.Api.Data.AppDbContext context,
        string? userId,
        string connectionId = "conn-1")
    {
        var claims = string.IsNullOrEmpty(userId)
            ? Array.Empty<Claim>()
            : new[] { new Claim(ClaimTypes.NameIdentifier, userId) };

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));

        var hubContextMock = new Mock<HubCallerContext>();
        hubContextMock.Setup(c => c.User).Returns(principal);
        hubContextMock.Setup(c => c.ConnectionId).Returns(connectionId);

        var groupsMock = new Mock<IGroupManager>();

        var hub = new ChatHub(context)
        {
            Context = hubContextMock.Object,
            Groups = groupsMock.Object
        };

        return (hub, groupsMock);
    }

    [Fact]
    public async Task JoinConversation_ParticipantUser_AddsConnectionToGroup()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Conversations.Add(new Conversation { Id = 5, User1Id = "user-1", User2Id = "user-2", ItemId = 1 });
        await context.SaveChangesAsync();

        var (hub, groupsMock) = CreateChatHub(context, "user-1", "conn-123");

        await hub.JoinConversation(5);

        groupsMock.Verify(
            g => g.AddToGroupAsync("conn-123", "conversation_5", default),
            Times.Once);
    }

    [Fact]
    public async Task JoinConversation_NonParticipantUser_ThrowsHubException()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Conversations.Add(new Conversation { Id = 5, User1Id = "user-1", User2Id = "user-2", ItemId = 1 });
        await context.SaveChangesAsync();

        var (hub, _) = CreateChatHub(context, "intruder-99");

        await Assert.ThrowsAsync<HubException>(() => hub.JoinConversation(5));
    }

    [Fact]
    public async Task JoinConversation_UnauthenticatedUser_ThrowsHubException()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var (hub, _) = CreateChatHub(context, null);

        await Assert.ThrowsAsync<HubException>(() => hub.JoinConversation(5));
    }

    [Fact]
    public async Task LeaveConversation_ValidConnection_RemovesConnectionFromGroup()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var (hub, groupsMock) = CreateChatHub(context, "user-1", "conn-123");

        await hub.LeaveConversation(5);

        groupsMock.Verify(
            g => g.RemoveFromGroupAsync("conn-123", "conversation_5", default),
            Times.Once);
    }
}