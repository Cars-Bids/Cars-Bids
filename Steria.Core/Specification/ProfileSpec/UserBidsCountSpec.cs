using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ProfileSpec;

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