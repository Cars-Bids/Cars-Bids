using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.CommonSpec;


namespace CarsAndBids.Core.Specification.WishlistSpec;

public class PagedWishlistsSpec : PagedSpec<Wishlist>
{
    public PagedWishlistsSpec(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query.OrderBy(bs => bs.AddedAt); // sort (unbonded)

    }
}
