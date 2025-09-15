using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Steria.Core.Interfaces;
using System.Security.Claims;

namespace Steria.API.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuctionHub(IAuctionService auctionService) : Hub
{
    public async Task PlaceBid(int auctionId, decimal amount)
    {
        var (userId, userName) = GetUserIdAndName();

        if (userId is null || string.IsNullOrEmpty(userName))
        {
            await Clients.Caller.SendAsync("BidRejected", "You are not authorized to participate in the auction");
            return;
        }

        var (isSuccess, error) = await auctionService.TryPlaceBid(auctionId, amount, userName, userId.Value);

        if (!isSuccess)
        {
            await Clients.Caller.SendAsync("BidRejected", error);
            return;
        }

        var auction = await auctionService.GetById(auctionId);

        await Clients.Caller.SendAsync("BidPlaced",
            auctionId,
            auction!.CurrentPrice,
            auction.CurrentBidder ?? "unknown",
            auction.EndTime
        );

        await Clients.OthersInGroup($"auction-{auctionId}").SendAsync("NewBidReceived",
            auctionId,
            auction!.CurrentPrice,
            auction.CurrentBidder ?? "unknown",
            auction.EndTime
        );
    }

    public override async Task OnConnectedAsync()
    {
        var auctionId = Context.GetHttpContext()?.Request.Query["auctionId"];

        if (int.TryParse(auctionId, out int id))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"auction-{id}");

            var auction = await auctionService.GetById(id);

            if (auction is not null)
            {
                await Clients.Caller.SendAsync("AuctionConnected",
                    auction.Id,
                    auction.CurrentPrice,
                    auction.CurrentBidder ?? "",
                    auction.EndTime
                );
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var auctionId = Context.GetHttpContext()?.Request.Query["auctionId"];
        
        if (int.TryParse(auctionId, out int id))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"auction-{id}");
        }

        var (userId, userName) = GetUserIdAndName();

        if (userId is not null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-auctions-{userId}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    private (int?, string?) GetUserIdAndName()
    {
        int? userId = int.TryParse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var res) ? res : null;
        string? userName = Context.User?.FindFirst("username")?.Value;
        return (userId, userName);
    }
}
