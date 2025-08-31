using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Data.Persistence.Seed;

public class BidSeeder(IGenericRepository<Bid> bidRepository,
                       IGenericRepository<Auction> auctionRepository,   
                       UserManager<User> userManager)           
{
    public async Task SeedAsync()
    {
        var existing = await bidRepository.GetItemBySpec(new FirstRecordSpec<Bid>());
        if (existing is not null) return;
        
        var allUsers = await userManager.Users.ToListAsync();
        var auctions = await auctionRepository.GetAsync();
        
        var random = new Random();
        
        foreach (var auction in auctions)
        {
            auction.Bids = new List<Bid>();
            
            int numBids = random.Next(15, 25);

            decimal currentBid = auction.StartPrice;

            DateTime start = auction.StartTime;
            DateTime end = auction.Status == AuctionStatus.Active ? DateTime.UtcNow : auction.EndTime;
            TimeSpan duration = end - start;

            for (int i = 0; i < numBids; i++)
            {
                var user = allUsers[random.Next(allUsers.Count)];

                decimal increment = random.Next(100, 1000); // Random increment $100-999
                currentBid += increment;

                // Progressive time, spread out
                double progress = (double)i / numBids; // 0 to almost 1, last bid before end
                DateTime bidTime = start + TimeSpan.FromTicks((long)(duration.Ticks * progress));

                var bid = new Bid
                {
                    AuctionId = auction.Id,
                    UserId = user.Id,
                    BidAmount = currentBid,
                    BidTime = bidTime
                };

                auction.Bids.Add(bid);
            }

            // Update auction's CurrentPrice and CurrentBidder based on last bid
            if (auction.Bids.Any())
            {
                var lastBid = auction.Bids.OrderBy(b => b.BidTime).Last();
                auction.CurrentPrice = lastBid.BidAmount;
                auction.CurrentBidder = allUsers.First(u => u.Id == lastBid.UserId).UserName;
            }

            await auctionRepository.UpdateAsync(auction);
        }
    }
}