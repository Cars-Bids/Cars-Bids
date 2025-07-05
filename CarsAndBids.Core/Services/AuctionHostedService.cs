using CarsAndBids.Core.Hubs;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarsAndBids.Core.Services;

public class AuctionHostedService(
    IServiceScopeFactory scopeFactory, 
    IHubContext<AuctionHub> hubContext
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //check and close expired auctions
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionService>();

            var activeAuctions = await auctionService.GetAllActiveAuctions();

            foreach (var auction in activeAuctions)
            {
                if (auction.EndTime <= DateTime.UtcNow)
                {
                    var finalStatus = auction.CurrentBidder is null 
                        ? AuctionStatus.NotSold 
                        : AuctionStatus.Sold;
                    
                    auctionService.UpdateStatus(auction.Id, finalStatus);

                    await hubContext.Clients.Group(auction.Id.ToString()).SendAsync("AuctionEnded", new
                    {
                        AuctionId = auction.Id,
                        FinalBid = auction.CurrentBid,
                        Winner = auction.CurrentBidder
                    });
                }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
