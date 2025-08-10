using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Ardalis.Specification;

namespace Steria.Core.Interfaces;

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
    Task InsertRangeAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(object id);
    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task SaveAsync();
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitAsync(IDbContextTransaction transaction);
    Task RollbackAsync(IDbContextTransaction transaction);

    Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
}