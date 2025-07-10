using System.Linq.Expressions;
using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using CarsAndBids.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarsAndBids.Data.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    internal ApplicationDbContext context;
    internal DbSet<TEntity> dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync(cancellationToken);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<TResult>> GetListBySpec<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<TResult?> GetItemBySpec<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
    {
        return SpecificationEvaluator.Default.GetQuery(dbSet.AsQueryable(), specification);
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }
    
    public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        await dbSet.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(object id)
    {
        TEntity? entityToDelete = await dbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            await DeleteAsync(entityToDelete);
        }
        else
        {
            throw new ArgumentException($"Entity with id {id} not found.");
        }
    }

    public virtual async Task DeleteAsync(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
        await context.SaveChangesAsync();
    }
    
    public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
        }

        dbSet.RemoveRange(entities);
        await context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        await context.SaveChangesAsync();
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await context.Database.BeginTransactionAsync();
    }
    public async Task CommitAsync(IDbContextTransaction transaction)
    {
        await transaction.CommitAsync();
    }
    public async Task RollbackAsync(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }
}
