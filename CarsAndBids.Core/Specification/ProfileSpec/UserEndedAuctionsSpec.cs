using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Enums;
using CarsAndBids.Core.Specification.CommonSpec;

namespace CarsAndBids.Core.Specification.ProfileSpec;

public class UserEndedAuctionsSpec : PagedSpec<Auction>
{
    public UserEndedAuctionsSpec(int userId, int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query
            .Where(auction => auction.SellerId == userId && (auction.Status == AuctionStatus.Sold || auction.Status == AuctionStatus.Cancelled || auction.Status == AuctionStatus.NotSold))
            .Include(auction => auction.Car)
            .ThenInclude(car => car.Model)
            .ThenInclude(model => model.Make)
            .Include(auction => auction.Car)
            .ThenInclude(car => car.Images)
            .AsNoTracking();

        Query.OrderByDescending(auction => auction.EndTime);
    }
}