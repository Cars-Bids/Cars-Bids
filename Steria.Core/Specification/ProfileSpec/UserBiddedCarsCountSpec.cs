using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ProfileSpec;

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