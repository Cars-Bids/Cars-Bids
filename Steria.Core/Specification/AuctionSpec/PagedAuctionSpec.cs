using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.СommonSpec;

namespace CarsAndBids.Core.Specification.AuctionSpec;

public class PagedAuctionSpec : PagedSpec<Auction>
{
    public PagedAuctionSpec(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query.OrderByDescending(a => a.CreatedAt);
    }
}
