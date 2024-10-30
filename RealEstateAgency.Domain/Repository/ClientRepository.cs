using RealEstateAgency.Domain.Interface;
using System.Linq.Expressions;

namespace RealEstateAgency.Domain.Repository.Mock;

public class ClientRepository : IRepository<Client, int>
{
    private static readonly List<Client> _clients = [];
    private static int _currentId;

    public async Task<List<Client>> GetAsList()
    {
        return await Task.FromResult(_clients);
    }

    public async Task<List<Client>> GetAsList(Expression<Func<Client, bool>> predicate)
    {
        return await Task.FromResult(_clients.AsQueryable().Where(predicate).ToList());
    }

    public async Task Add(Client newRecord)
    {
        newRecord.Id = _currentId++;
        await Task.Run(() => _clients.Add(newRecord));
    }

    public async Task Delete(int key)
    {
        var client = _clients.FirstOrDefault(c => c.Id == key);
        if (client != null)
        {
            await Task.Run(() => _clients.Remove(client));
        }
    }

    public async Task Update(Client newValue)
    {
        var client = _clients.FirstOrDefault(c => c.Id == newValue.Id);
        if (client != null)
        {
            await Task.Run(() =>
            {
                client.FirstAndLastName = newValue.FirstAndLastName;
                client.Pasport = newValue.Pasport;
                client.NumberPhone = newValue.NumberPhone;
                client.Address = newValue.Address;
                client.Email = newValue.Email;
            });
        }
    }
}
