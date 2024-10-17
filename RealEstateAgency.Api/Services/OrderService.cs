using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Api.Services;

public class OrderService(IRepository<Order, int> orderRepository, IMapper mapper)
{
    public async Task<List<OrderDto>> GetAllOrders()
    {
        var orders = await orderRepository.GetAsList();
        return mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<List<OrderDto>> GetOrdersByPredicate(Func<Order, bool> predicate)
    {
        var orders = await orderRepository.GetAsList(predicate);
        return mapper.Map<List<OrderDto>>(orders);
    }

    public async Task AddOrder(OrderDto orderDto)
    {
        var order = mapper.Map<Order>(orderDto);
        await orderRepository.Add(order);
    }

    public async Task UpdateOrder(int id, OrderDto orderDto)
    {
        var allOrder = await orderRepository.GetAsList();
        if (!allOrder.Any(l => l.Id == id))
        {
            throw new ArgumentException("Неправильный ID");
        }
        var order = mapper.Map<Order>(orderDto);
        order.Id = id;
        await orderRepository.Update(order);
    }

    public async Task DeleteOrder(int orderId)
    {
        await orderRepository.Delete(orderId);
    }
}
