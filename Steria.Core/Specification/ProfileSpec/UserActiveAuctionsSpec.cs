using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.ProfileSpec;

public class UserActiveAuctionsSpec : PagedSpec<Auction>
{
    public UserActiveAuctionsSpec(int userId, int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query
            .Where(auction => auction.SellerId == userId && auction.Status == AuctionStatus.Active)
            .Include(auction => auction.Car)
                .ThenInclude(car => car.Model)
                    .ThenInclude(model => model.Make)
            .Include(auction => auction.Car)
                .ThenInclude(car => car.BodyStyle)
            .Include(auction => auction.Car)
                .ThenInclude(car => car.Images.Where(img => img.ImageCategory == ImageCategory.Main).Take(1))
            .AsNoTracking();

        Query.OrderByDescending(auction => auction.CreatedAt);
    }
}