using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Data;
using RealEstateAgency.Domain.Interface;
using System.Linq.Expressions;

namespace RealEstateAgency.Domain.Repository;

public class OrderRepository(RealEstateAgencyContext context) : IRepository<Order, int>
{
    public async Task<List<Order>> GetAsList()
    {
        return await context.Orders.ToListAsync();
    }

    public async Task<List<Order>> GetAsList(Expression<Func<Order, bool>> predicate)
    {
        return await context.Orders.Where(predicate).ToListAsync();
    }

    public async Task Add(Order newRecord)
    {
        await context.Orders.AddAsync(newRecord);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int key)
    {
        var order = await context.Orders.FindAsync(key);
        if (order != null)
        {
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(Order newValue)
    {
        context.Orders.Update(newValue);
        await context.SaveChangesAsync();
    }
}
