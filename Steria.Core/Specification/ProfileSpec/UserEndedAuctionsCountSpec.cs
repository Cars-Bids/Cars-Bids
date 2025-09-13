using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Specification.CommonSpec;

namespace Steria.Core.Specification.ProfileSpec;

public class UserEndedAuctionsCountSpec : CountSpec<Auction>
{
    public UserEndedAuctionsCountSpec(int userId)
    {
        Query
            .Where(auction => auction.SellerId == userId &&
                (auction.Status == AuctionStatus.Sold ||
                 auction.Status == AuctionStatus.Cancelled ||
                 auction.Status == AuctionStatus.NotSold))
            .AsNoTracking();
    }
}