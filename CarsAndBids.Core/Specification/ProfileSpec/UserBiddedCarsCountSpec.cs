using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Specification.Profile;

public class UserBiddedCarsCountSpec : Specification<Bid>
{
    public UserBiddedCarsCountSpec(int userId)
    {
        Query
            .Where(bid => bid.UserId == userId)
            .Include(bid => bid.Auction)
            .AsNoTracking();
    }
}