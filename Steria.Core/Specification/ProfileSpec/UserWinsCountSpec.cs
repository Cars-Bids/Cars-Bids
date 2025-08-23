using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Specification.ProfileSpec;

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