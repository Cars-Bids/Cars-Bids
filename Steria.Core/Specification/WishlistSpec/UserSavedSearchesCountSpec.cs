using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.WishlistSpec;

public class UserSavedSearchesCountSpec : Specification<SavedSearch>
{
    public UserSavedSearchesCountSpec(int userId)
    {
        Query.Where(s => s.UserId == userId);
    }
}