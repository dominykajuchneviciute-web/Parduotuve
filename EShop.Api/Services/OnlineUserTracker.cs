using System.Collections.Concurrent;

namespace EShop.Api.Services;

public class OnlineUserTracker
{
    private readonly ConcurrentDictionary<string, string> _userIdByConnectionId = new();

    public void AddConnection(string connectionId, string userId)
    {
        _userIdByConnectionId[connectionId] = userId;
    }

    public void RemoveConnection(string connectionId)
    {
        _userIdByConnectionId.TryRemove(connectionId, out _);
    }
    public List<string> GetOnlineUserIds()
    {
        return _userIdByConnectionId.Values.Distinct().ToList();
    }

    public int GetOnlineUserCount()
    {
        return GetOnlineUserIds().Count;
    }
}