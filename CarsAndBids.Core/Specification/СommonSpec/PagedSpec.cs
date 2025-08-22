using Ardalis.Specification;

namespace CarsAndBids.Core.Specification.CommonSpec;

public abstract class PagedSpec<TEntity> : Specification<TEntity, TEntity> where TEntity : class
{
    protected PagedSpec(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;

        Query
            .AsNoTracking()
            .Skip(skip)
            .Take(pageSize)
            .Select(x => x);
    }
}
