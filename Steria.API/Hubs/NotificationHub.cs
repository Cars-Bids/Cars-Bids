using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Steria.Core.CQRS.Notification;
using Steria.Core.Interfaces;

namespace Steria.API.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationHub(IConnectionManager<NotificationHub> connectionManager,
                             IMediator mediator) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId(Context);
        connectionManager.AddConnection(userId, Context.ConnectionId);

        await Clients.Caller.SendAsync("UnreadNotifications",
            await mediator.Send(new GetUnreadNotificationsQuery { UserId = userId }));
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        connectionManager.RemoveConnection(Context.ConnectionId);
        
        await base.OnDisconnectedAsync(exception);
    }

    private int GetUserId(HubCallerContext context)
    {
        return int.Parse(context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
    
    private string? GetUsername(HubCallerContext context)
    {
        return context.User?.FindFirst("username")?.Value;
    }
}