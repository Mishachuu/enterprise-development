using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Data;
using RealEstateAgency.Domain.Interface;
using System.Linq.Expressions;

namespace RealEstateAgency.Domain.Repository;

public class OrderRepository(RealEstateAgencyContext context) : IRepository<Order, int>
{
    public async Task<List<Order>> GetAsList()
    {
        var queryWithIncludes = context.Orders
            .Include(o => o.Client)
            .Include(o => o.RealEstate);
        return await queryWithIncludes.ToListAsync();
    }

    public async Task<List<Order>> GetAsList(Expression<Func<Order, bool>> predicate)
    {
        var queryWithIncludes = context.Orders
            .Include(o => o.Client)
            .Include(o => o.RealEstate);

        var filteredQuery = queryWithIncludes.Where(predicate);

        var orders = await filteredQuery.ToListAsync();


        return orders;
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
        var order = await context.Orders.FindAsync(newValue.Id);
        if (order != null)
        {
            order.Price = newValue.Price;
            order.RealEstate = newValue.RealEstate;
            order.Client = newValue.Client;
            order.Time = newValue.Time;

            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
    }
}
