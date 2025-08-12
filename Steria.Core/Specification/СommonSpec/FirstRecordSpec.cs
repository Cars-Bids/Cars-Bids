using Ardalis.Specification;

namespace Steria.Core.Specification.СommonSpec;

public class FirstRecordSpec<TEntity> : Specification<TEntity, TEntity> 
{
    public FirstRecordSpec()
    {
        Query.Take(1)
            .Select(x => x);

    }
}