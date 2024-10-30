using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace RealEstateAgency.Domain.Repository
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly RealEstateAgencyContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(RealEstateAgencyContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

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
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TKey key)
        {
            var entity = await _dbSet.FindAsync(key);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(TEntity newValue)
        {
            _dbSet.Update(newValue);
            await _context.SaveChangesAsync();
        }
    }
}
