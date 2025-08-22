using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Specification.Profile;

public class UserBidsCountSpec : Specification<Bid, int>
{
    public UserBidsCountSpec(int userId)
    {
        Query
            .Where(bid => bid.UserId == userId)
            .AsNoTracking();

        Query.Select(bid => bid.Id);
    }
}