using System.Linq.Expressions;
using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using CarsAndBids.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    // Метод для специфікацій
    public async Task<object?> GetWithSpecificationAsync(
        ISpecification<TEntity>? spec = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = dbSet;

        if (spec != null)
        {
            query = SpecificationEvaluator.Default.GetQuery(query, spec);

            if (spec.GetType().IsGenericType && spec.GetType().GetGenericTypeDefinition() == typeof(Specification<,>))
            {
                if (spec is ISingleResultSpecification)
                {
                    return await query.SingleOrDefaultAsync(cancellationToken);
                }
                return await query.ToListAsync(cancellationToken);
            }

            return await query.ToListAsync(cancellationToken);
        }

        return await query.ToListAsync(cancellationToken);
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

    public virtual async Task UpdateAsync(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
}
