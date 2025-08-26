using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.AuctionSpec;

public class PagedAuctionSpec : PagedSpec<Auction>
{
    public PagedAuctionSpec(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query.OrderByDescending(a => a.CreatedAt);
    }
}
