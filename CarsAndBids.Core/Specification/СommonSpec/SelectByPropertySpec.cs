using Ardalis.Specification;
using System.Linq.Expressions;

namespace CarsAndBids.Core.Specification.СommonSpec;

public class SelectByPropertySpec<TEntity, TResult> : Specification<TEntity, TResult>
    where TEntity : class
{
    private readonly Expression<Func<TEntity, bool>> _filter;
    private readonly Expression<Func<TEntity, TResult>> _selector;

    public SelectByPropertySpec(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector)
    {
        _filter = filter;
        _selector = selector;

        Query
            .Where(_filter)
            .Select(_selector);
    }
}