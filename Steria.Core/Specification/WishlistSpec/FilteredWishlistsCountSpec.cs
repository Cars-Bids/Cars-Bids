using Ardalis.Specification;
using Steria.Core.CQRS.Wishlists;
using Steria.Core.Entities;

namespace Steria.Core.Specification.WishlistSpec;

public class FilteredWishlistsCountSpec : Specification<Wishlist>
{
    public FilteredWishlistsCountSpec(GetFilteredWishlistsQuery query)
    {
        Query.Where(w => w.UserId == query.UserId);

        if (query.NewCars == true)
        {
            Query.Where(w => w.Auction.Car.Mileage < 10000);
        }
        if (query.Inspected == true)
        {
            Query.Where(w => w.Auction.IsInspected);
        }
    }
}