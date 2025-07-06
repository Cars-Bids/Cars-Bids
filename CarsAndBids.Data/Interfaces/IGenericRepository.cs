using System.Linq.Expressions;
using Ardalis.Specification;

namespace CarsAndBids.Data.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default);

    Task<List<TResult>> GetListBySpec<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);
    Task<TResult?> GetItemBySpec<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(object id);
    Task InsertAsync(TEntity entity);
    Task DeleteAsync(object id);
    Task DeleteAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
}