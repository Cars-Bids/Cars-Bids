using CarsAndBids.API.Hubs;
using CarsAndBids.Core.Enums;
using CarsAndBids.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CarsAndBids.API.HostedServices;

public class AuctionHostedService(
    IServiceScopeFactory scopeFactory,
    IHubContext<AuctionHub> hubContext
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionService>();

            var auctions = await auctionService.GetAllOpenedAuctions();

            foreach (var auction in auctions)
            {
                //start pending auctions
                if (auction.Status == AuctionStatus.Pending && auction.StartTime <= DateTime.UtcNow)
                {
                    auctionService.UpdateStatus(auction.Id, AuctionStatus.Active);

                    await hubContext.Clients.Group(auction.Id.ToString()).SendAsync(
                        "AuctionStarted",
                        new {
                            AuctionId = auction.Id,
                            StartPrice = auction.StartPrice,
                            EndTime = auction.EndTime
                        }, 
                        cancellationToken);
                }

                //finish expired auctions
                if (auction.Status == AuctionStatus.Active && auction.EndTime <= DateTime.UtcNow)
                {
                    var finalStatus = auction.CurrentPrice >= auction.StartPrice && auction.CurrentBidder is not null
                        ? AuctionStatus.Sold
                        : AuctionStatus.NotSold;

                    auctionService.UpdateStatus(auction.Id, finalStatus);

                    await hubContext.Clients.Group(auction.Id.ToString()).SendAsync(
                        "AuctionEnded",
                        new
                        {
                            AuctionId = auction.Id,
                            FinalBid = auction.CurrentPrice,
                            Winner = auction.CurrentBidder
                        },
                        cancellationToken);
                }
            }
            await Task.Delay(1000, cancellationToken);
        }
    }
}
