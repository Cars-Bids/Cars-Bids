using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Specification.Profile;

public class UserCommentsCountSpec : Specification<Comment>
{
    public UserCommentsCountSpec(int userId)
    {
        Query
            .Where(comment => comment.UserId == userId)
            .AsNoTracking();
    }
}