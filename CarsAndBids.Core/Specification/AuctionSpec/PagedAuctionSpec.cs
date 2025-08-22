using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.CommonSpec;

namespace CarsAndBids.Core.Specification.AuctionSpec;

public class PagedAuctionSpec : PagedSpec<Auction>
{
    public PagedAuctionSpec(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query.OrderByDescending(a => a.CreatedAt);
    }
}
