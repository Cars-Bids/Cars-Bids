using Microsoft.AspNetCore.SignalR;
using Steria.API.Hubs;
using Steria.Core.Enums;
using Steria.Core.Interfaces;

namespace Steria.API.Notifiers;

public class SignalRNotifier(IHubContext<NotificationHub> hubContext,
                             IConnectionManager<NotificationHub> connectionManager) : IRealtimeNotifier
{
    public async Task SendToUserAsync(int userId, NotificationSource source, object payload)
    {
        var connections = connectionManager.GetConnections(userId);
        foreach (var connection in connections)
        {
            await hubContext.Clients.Client(connection).SendAsync("ReceiveNotification", source, payload);
        }
    }
}