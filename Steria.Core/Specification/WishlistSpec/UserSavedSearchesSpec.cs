using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.WishlistSpec;

public class UserSavedSearchesSpec : Specification<SavedSearch, SavedSearch>
{
    public UserSavedSearchesSpec(int userId, int pageNumber, int pageSize)
    {
        Query
            .Where(s => s.UserId == userId)
            .Include(s => s.Make)
            .Include(s => s.Model)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();

        Query.Select(s => s);
    }
}
