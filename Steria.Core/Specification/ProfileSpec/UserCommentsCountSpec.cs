using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.ProfileSpec;

public class UserCommentsCountSpec : Specification<Comment>
{
    public UserCommentsCountSpec(int userId)
    {
        Query
            .Where(comment => comment.UserId == userId)
            .AsNoTracking();
    }
}