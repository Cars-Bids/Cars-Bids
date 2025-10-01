using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.AuctionSpec;

public class LastBidderIdSpec : Specification<Auction, int>
{
    public LastBidderIdSpec(int auctionId)
    {
        Query
            .Where(x => x.Id == auctionId)
            .Select(x => x.Bids!
                .OrderByDescending(b => b.BidTime)
                .Select(b => b.UserId)
                .FirstOrDefault()
            );
    }
}