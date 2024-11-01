using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Data;
using RealEstateAgency.Domain.Interface;
using System.Linq.Expressions;

namespace RealEstateAgency.Domain.Repository;

public class RealEstateRepository(RealEstateAgencyContext context) : IRepository<RealEstate, int>
{
    public async Task<List<RealEstate>> GetAsList()
    {
        return await context.RealEstates.ToListAsync();
    }

    public async Task<List<RealEstate>> GetAsList(Expression<Func<RealEstate, bool>> predicate)
    {
        return await context.RealEstates.Where(predicate).ToListAsync();
    }

    public async Task Add(RealEstate newRecord)
    {
        await context.RealEstates.AddAsync(newRecord);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int key)
    {
        var realEstate = await context.RealEstates.FindAsync(key);
        if (realEstate != null)
        {
            context.RealEstates.Remove(realEstate);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(RealEstate newValue)
    {
        context.RealEstates.Update(newValue);
        await context.SaveChangesAsync();
    }
}
