using Ardalis.Specification;

namespace Steria.Core.Specification.CommonSpec;

public abstract class CountSpec<TEntity> : Specification<TEntity, TEntity> where TEntity : class
{
    protected CountSpec()
    {
        Query
            .AsNoTracking()
            .Select(x => x);
    }
}