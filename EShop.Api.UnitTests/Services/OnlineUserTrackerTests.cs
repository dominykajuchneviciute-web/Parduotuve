using EShop.Api.Services;

namespace EShop.Api.UnitTests.Services;

public class OnlineUserTrackerTests
{
    [Fact]
    public void AddAndRemoveConnections_TracksDistinctOnlineUsers()
    {
        var tracker = new OnlineUserTracker();

        tracker.AddConnection("conn-1", "user-1");
        tracker.AddConnection("conn-2", "user-1");
        tracker.AddConnection("conn-3", "user-2");

        Assert.Equal(2, tracker.GetOnlineUserCount());

        tracker.RemoveConnection("conn-1");
        Assert.Equal(2, tracker.GetOnlineUserCount());

        tracker.RemoveConnection("conn-2");
        Assert.Equal(1, tracker.GetOnlineUserCount());
    }
}