using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Specification.ProfileSpec;

public class UserAuctionsSpec : Specification<Auction, Auction>
{
    public UserAuctionsSpec(int userId)
    {
        Query
            .Where(auction => auction.SellerId == userId && auction.Status == AuctionStatus.Active)
            .Include(auction => auction.Car)
            .ThenInclude(car => car.Model)
            .ThenInclude(model => model.Make)
            .Include(auction => auction.Car)
            .ThenInclude(car => car.Images)
            .AsNoTracking();

        Query.Select(auction => auction);
    }
}