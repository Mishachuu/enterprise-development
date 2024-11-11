using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Data;
using RealEstateAgency.Domain.Interface;
using System.Linq.Expressions;

namespace RealEstateAgency.Domain.Repository;

public class ClientRepository(RealEstateAgencyContext context) : IRepository<Client, int>
{
    public async Task<List<Client>> GetAsList()
    {
        return await context.Clients.ToListAsync();
    }

    public async Task<List<Client>> GetAsList(Expression<Func<Client, bool>> predicate)
    {
        return await context.Clients.Where(predicate).ToListAsync();
    }

    public async Task Add(Client newRecord)
    {
        await context.Clients.AddAsync(newRecord);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int key)
    {
        var client = await context.Clients.FindAsync(key);
        if (client != null)
        {
            context.Clients.Remove(client);
            await context.SaveChangesAsync();
        }
    }

    public async Task Update(Client newValue)
    {
        var client = await context.Clients.FindAsync(newValue.Id);
        if (client != null)
        {
            client.Email = newValue.Email;
            client.Address = newValue.Address;
            client.NumberPhone = newValue.NumberPhone;
            client.FirstAndLastName = newValue.FirstAndLastName;
            client.Pasport = newValue.Pasport;

            context.Clients.Update(client);
            await context.SaveChangesAsync();
        }
    }
}
