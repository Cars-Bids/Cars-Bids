using Microsoft.AspNetCore.SignalR;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.DTOs;
using AutoMapper;
using CarsAndBids.Core.Enums;

namespace CarsAndBids.API.Hubs;

public class AuctionHub(IAuctionService auctionService, IMapper mapper) : Hub
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

    public async Task SubscribeToUserAuctions()
    {
        var userId = Context.User?.FindFirst("nameid")?.Value;
        if (!int.TryParse(userId, out var parsedUserId))
        {
            await Clients.Caller.SendAsync("SubscriptionFailed", "You are not authorized");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-auctions-{parsedUserId}");

        var auctions = await auctionService.GetUserAuctions(parsedUserId);
        await Clients.Caller.SendAsync("ReceiveUserAuctions", auctions.Select(a => new
        {
            AuctionId = a.Id,
            CarId = a.CarId,
            StartPrice = a.StartPrice,
            CurrentPrice = a.CurrentPrice,
            CurrentBidder = a.CurrentBidder,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status,
            Timestamp = DateTime.UtcNow,
            Car = new
            {
                Year = a.Car.Year,
                Make = a.Car.Model.Make.Name,
                Model = a.Car.Model.Name,
                ExteriorColor = a.Car.ExteriorColor,
                Mileage = a.Car.Mileage,
                MainImage = a.Car.Images
                    .Where(img => img.ImageCategory == ImageCategory.Main)
                    .Select(img => img.ImageUrl)
                    .FirstOrDefault() ?? a.Car.Images
                    .Select(img => img.ImageUrl)
                    .FirstOrDefault() ?? ""
            }
        }));
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

        var userId = Context.User?.FindFirst("nameid")?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-auctions-{userId}");
        }

        await base.OnDisconnectedAsync(exception);
    }
}