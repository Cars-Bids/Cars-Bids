using Microsoft.AspNetCore.SignalR;
using Steria.Core.Interfaces;

namespace Steria.Data.Services;

public class ConnectionManager<THub> : IConnectionManager<THub> where THub : Hub
{
    private readonly Dictionary<int, List<string>> _userConnections = new();

    public void AddConnection(int userId, string connectionId)
    {
        lock (_userConnections)
        {
            if (!_userConnections.ContainsKey(userId))
                _userConnections[userId] = new List<string>();

            if (!_userConnections[userId].Contains(connectionId))
                _userConnections[userId].Add(connectionId);
        }
    }

    public void RemoveConnection(string connectionId)
    {
        lock (_userConnections)
        {
            var item = _userConnections
                .FirstOrDefault(x => x.Value.Contains(connectionId));

            if (item.Key != 0)
            {
                item.Value.Remove(connectionId);
                if (!item.Value.Any())
                    _userConnections.Remove(item.Key);
            }
        }
    }

    public IReadOnlyList<string> GetConnections(int userId)
    {
        lock (_userConnections)
        {
            return _userConnections.TryGetValue(userId, out var connections)
                ? connections.AsReadOnly()
                : Array.Empty<string>();
        }
    }
}
