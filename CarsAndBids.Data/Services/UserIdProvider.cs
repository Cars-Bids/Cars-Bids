namespace CarsAndBids.Data.Services;

using Microsoft.AspNetCore.SignalR;

public class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst("nameid")?.Value;
    }
}
