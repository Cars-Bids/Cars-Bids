using Steria.API.Hubs;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Steria.API.HostedServices;

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
                if (auction.Status is AuctionStatus.Pending && auction.StartTime <= DateTime.UtcNow)
                {
                    auctionService.UpdateStatus(auction.Id, AuctionStatus.Active);

                    await hubContext.Clients.Group($"auction-{auction.Id}").SendAsync("AuctionStarted",
                        auction.Id,
                        auction.StartPrice,
                        auction.EndTime, 
                    cancellationToken);
                }

                //finish expired auctions
                if (auction.Status is AuctionStatus.Active && auction.EndTime <= DateTime.UtcNow)
                {
                    var finalStatus = auction.CurrentPrice >= auction.StartPrice && auction.CurrentBidder is not null
                        ? AuctionStatus.Sold
                        : AuctionStatus.NotSold;

                    auctionService.UpdateStatus(auction.Id, finalStatus);

                    await hubContext.Clients.Group($"auction-{auction.Id}").SendAsync("AuctionFinished",
                        auction.Id,
                        auction.CurrentPrice,
                        auction.CurrentBidder ?? "no winner",
                        auction.EndTime,
                    cancellationToken);
                }
            }
            await Task.Delay(1000, cancellationToken);
        }
    }
}
