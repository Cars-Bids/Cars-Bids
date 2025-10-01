using Ardalis.Specification;
using Steria.Core.CQRS.Wishlists;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Specification.WishlistSpec;

public class FilteredWishlistsSpec : Specification<Wishlist, Wishlist>
{
    public FilteredWishlistsSpec(GetFilteredWishlistsQuery query)
    {
        Query
            .Where(w => w.UserId == query.UserId)
            .Include(w => w.Auction)
            .ThenInclude(a => a.Car)
            .ThenInclude(c => c.Model)
            .ThenInclude(m => m.Make)
            .Include(w => w.Auction)
            .ThenInclude(a => a.Car.Images.Where(img => img.ImageCategory == ImageCategory.Main))
            .AsNoTracking();

        if (query.NewCars == true)
        {
            Query.Where(w => w.Auction.Car.Mileage < 10000);
        }
        if (query.Inspected == true)
        {
            Query.Where(w => w.Auction.IsInspected);
        }

        if (query.EndingSoon == true)
        {
            Query.OrderBy(w => w.Auction.EndTime);
        }
        else
        {
            Query.OrderByDescending(w => w.AddedAt);
        }

        Query.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);

        Query.Select(w => w);
    }
}
