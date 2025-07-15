using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.СommonSpec;


namespace CarsAndBids.Data.Persistence.Repositories.Specification.WishlistSpec;

public class PagedWishlistsSpec : PagedSpec<Wishlist>
{
    public PagedWishlistsSpec(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query.OrderBy(bs => bs.AddedAt); // sort (unbonded)

    }
}
