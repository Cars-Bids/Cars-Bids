using Microsoft.AspNetCore.SignalR;

namespace Steria.Core.Interfaces;

public interface IConnectionManager<THub> where THub : Hub
{
    void AddConnection(int userId, string connectionId);
    void RemoveConnection(string connectionId);
    IReadOnlyList<string> GetConnections(int userId);
}
