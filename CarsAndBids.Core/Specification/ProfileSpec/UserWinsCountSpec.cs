using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Enums;

namespace CarsAndBids.Core.Specification.Profile;

public class UserWinsCountSpec : Specification<Auction, int>
{
    public UserWinsCountSpec(int userId)
    {
        Query
            .Where(auction => auction.CurrentBidder == auction.SellerId.ToString() && auction.Status == AuctionStatus.Sold && auction.SellerId == userId)
            .AsNoTracking();

        Query.Select(auction => auction.Id);
    }
}