using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;


namespace Steria.Core.Specification.WishlistSpec;

public class PagedWishlistsSpec : PagedSpec<Wishlist>
{
    public PagedWishlistsSpec(int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query.OrderBy(bs => bs.AddedAt); // sort (unbonded)

    }
}
