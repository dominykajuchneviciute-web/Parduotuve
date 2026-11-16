using System.Collections.Concurrent;

namespace EShop.Api.Services;

public class OnlineUserTracker
{
    private readonly ConcurrentDictionary<string, string> _connections = new();

    public void AddConnection(string connectionId, string userId)
    {
        _connections[connectionId] = userId;
    }

    public void RemoveConnection(string connectionId)
    {
        _connections.TryRemove(connectionId, out _);
    }

    public int GetOnlineUserCount()
    {
        return _connections.Values.Distinct().Count();
    }
}