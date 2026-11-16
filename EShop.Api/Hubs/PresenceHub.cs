using EShop.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EShop.Api.Hubs;

[Authorize]
public class PresenceHub : Hub
{
    private readonly OnlineUserTracker _tracker;

    public PresenceHub(OnlineUserTracker tracker)
    {
        _tracker = tracker;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            _tracker.AddConnection(Context.ConnectionId, userId);
        }

        await Clients.All.SendAsync("OnlineCountChanged", _tracker.GetOnlineUserCount());
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _tracker.RemoveConnection(Context.ConnectionId);

        await Clients.All.SendAsync("OnlineCountChanged", _tracker.GetOnlineUserCount());
        await base.OnDisconnectedAsync(exception);
    }
}