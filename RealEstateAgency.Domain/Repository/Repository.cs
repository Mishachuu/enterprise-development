using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Domain.Interface;
using System.Linq.Expressions;

namespace RealEstateAgency.Domain.Repository;

public class Repository<TEntity, TKey>(RealEstateAgencyContext context) : IRepository<TEntity, TKey>
    where TEntity : class
{
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<List<TEntity>> GetAsList()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<List<TEntity>> GetAsList(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task Add(TEntity newRecord)
    {
        await _dbSet.AddAsync(newRecord);
        await context.SaveChangesAsync();
    }

    public async Task Delete(TKey key)
    {
        var entity = await _dbSet.FindAsync(key);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(TEntity newValue)
    {
        _dbSet.Update(newValue);
        await context.SaveChangesAsync();
    }
}
