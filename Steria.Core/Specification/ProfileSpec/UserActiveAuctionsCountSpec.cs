using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Specification.CommonSpec;

namespace Steria.Core.Specification.ProfileSpec;

public class UserActiveAuctionsCountSpec : CountSpec<Auction>
{
    public UserActiveAuctionsCountSpec(int userId)
    {
        Query
            .Where(auction => auction.SellerId == userId && auction.Status == AuctionStatus.Active)
            .AsNoTracking();
    }
}