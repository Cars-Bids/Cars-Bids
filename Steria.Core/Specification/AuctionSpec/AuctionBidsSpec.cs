using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.AuctionSpec;

public class AuctionBidsSpec : PagedSpec<Bid>
{
    public AuctionBidsSpec(int auctionId, int pageNumber, int pageSize) 
        : base(pageNumber, pageSize)
    {
        Query.Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.BidTime)
            .ThenByDescending(b => b.Id)
            .Include(b => b.User);
    }
}