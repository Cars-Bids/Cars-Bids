using System.Linq.Expressions;
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

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
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
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
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
