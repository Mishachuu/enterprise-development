using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Domain.Repository.Mock;

public class OrderRepository : IRepository<Order, int>
{
    private static readonly List<Order> _orders = [];
    private static int _currentId;


    public async Task<List<Order>> GetAsList()
    {
        return await Task.FromResult(_orders);
    }

    public async Task<List<Order>> GetAsList(Func<Order, bool> predicate)
    {
        return await Task.FromResult(_orders.Where(predicate).ToList());
    }


    public async Task Add(Order newRecord)
    {
        newRecord.Id = _currentId++;
        await Task.Run(() => _orders.Add(newRecord));
    }

    public async Task Delete(int key)
    {
        var order = _orders.FirstOrDefault(o => o.Id == key);
        if (order != null)
        {
            await Task.Run(() => _orders.Remove(order));
        }
    }

    public async Task Update(Order newValue)
    {
        var order = _orders.FirstOrDefault(o => o.Id == newValue.Id);
        if (order != null)
        {
            await Task.Run(() =>
            {
                order.Time = newValue.Time;
                order.Id = newValue.Id;
                order.Price = newValue.Price;
            });
        }
    }
}
