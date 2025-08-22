using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.CommonSpec;

namespace CarsAndBids.Core.Specification.Profile;

public class UserBiddedCarsSpec : PagedSpec<Bid>
{
    public UserBiddedCarsSpec(int userId, int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query
            .Where(bid => bid.UserId == userId)
            .Include(bid => bid.Auction)
            .ThenInclude(auction => auction.Car)
            .ThenInclude(car => car.Model)
            .ThenInclude(model => model.Make)
            .Include(bid => bid.Auction)
            .ThenInclude(auction => auction.Car)
            .ThenInclude(car => car.BodyStyle)
            .Include(bid => bid.Auction)
            .ThenInclude(auction => auction.Car)
            .ThenInclude(car => car.Images)
            .AsNoTracking();

        Query.OrderByDescending(bid => bid.BidTime);
    }
}