using EShop.Api.Hubs;
using EShop.Api.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace EShop.Api.UnitTests.Hubs;

public class PresenceHubTests
{
    private static (PresenceHub hub, Mock<IClientProxy> allClientsMock) CreatePresenceHub(
        OnlineUserTracker tracker,
        string? userId,
        string connectionId = "conn-1")
    {
        var hubContextMock = new Mock<HubCallerContext>();
        hubContextMock.Setup(c => c.UserIdentifier).Returns(userId);
        hubContextMock.Setup(c => c.ConnectionId).Returns(connectionId);

        var allClientsMock = new Mock<IClientProxy>();
        var clientsMock = new Mock<IHubCallerClients>();
        clientsMock.Setup(c => c.All).Returns(allClientsMock.Object);

        var hub = new PresenceHub(tracker)
        {
            Context = hubContextMock.Object,
            Clients = clientsMock.Object
        };

        return (hub, allClientsMock);
    }

    [Fact]
    public async Task OnConnectedAsync_AuthenticatedUser_AddsUserAndBroadcastsCount()
    {
        var tracker = new OnlineUserTracker();
        var (hub, allClientsMock) = CreatePresenceHub(tracker, "user-1", "conn-1");

        await hub.OnConnectedAsync();

        Assert.Equal(1, tracker.GetOnlineUserCount());
        allClientsMock.Verify(
            c => c.SendCoreAsync("OnlineCountChanged", It.Is<object?[]>(args => (int)args[0]! == 1), default),
            Times.Once);
    }

    [Fact]
    public async Task OnDisconnectedAsync_ConnectedUser_RemovesUserAndBroadcastsUpdatedCount()
    {
        var tracker = new OnlineUserTracker();
        tracker.AddConnection("conn-1", "user-1");
        var (hub, allClientsMock) = CreatePresenceHub(tracker, "user-1", "conn-1");

        await hub.OnDisconnectedAsync(null);

        Assert.Equal(0, tracker.GetOnlineUserCount());
        allClientsMock.Verify(
            c => c.SendCoreAsync("OnlineCountChanged", It.Is<object?[]>(args => (int)args[0]! == 0), default),
            Times.Once);
    }
}