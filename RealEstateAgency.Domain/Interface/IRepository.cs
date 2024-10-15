namespace RealEstateAgency.Domain.Interface;

public interface IRepository<TEntity, TKey>
{
    Task<List<TEntity>> GetAsList();
    Task<List<TEntity>> GetAsList(Func<TEntity, bool> predicate);
    Task Add(TEntity newRecord);
    Task Delete(TKey key);
    Task Update(TEntity newValue);
}
