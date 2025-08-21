using CarsAndBids.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CarsAndBids.API.Hubs;

public class AuctionHub(IAuctionService auctionService) : Hub
{
    public async Task PlaceBid(int auctionId, decimal amount)
    {
        var bidderId = Context.User?.FindFirst("nameid")?.Value;
        var bidderName = Context.User?.FindFirst("username")?.Value;

        if (!int.TryParse(bidderId, out var userId) || string.IsNullOrEmpty(bidderName))
        {
            await Clients.Caller.SendAsync("BidRejected", "You are not authorized to participate in the auction");
            return;
        }

        var (isSuccess, error) = await auctionService.TryPlaceBid(auctionId, amount, bidderName, userId);

        if (!isSuccess)
        {
            await Clients.Caller.SendAsync("BidRejected", error);
            return;
        }

        var auction = await auctionService.GetById(auctionId);

        await Clients.Group(auctionId.ToString()).SendAsync("ReceiveBid", new
        {
            AuctionId = auctionId,
            CurrentPrice = auction!.CurrentPrice,
            CurrentBidder = auction.CurrentBidder,
            EndTime = auction.EndTime,
            Timestamp = DateTime.UtcNow
        });
    }

    public override async Task OnConnectedAsync()
    {
        var auctionId = Context.GetHttpContext()?.Request.Query["auctionId"];

        if (int.TryParse(auctionId, out int id))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, auctionId!);

            var auction = await auctionService.GetById(id);
            
            if (auction is not null)
            {
                await Clients.Caller.SendAsync("ConnectAuction", new
                {
                    AuctionId = auction.Id,
                    StartPrice = auction.StartPrice,
                    CurrentPrice = auction.CurrentPrice,
                    CurrentBidder = auction.CurrentBidder,
                    EndTime = auction.EndTime,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var auctionId = Context.GetHttpContext()?.Request.Query["auctionId"];
        if (!string.IsNullOrEmpty(auctionId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, auctionId!);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
