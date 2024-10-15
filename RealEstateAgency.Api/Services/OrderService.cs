using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Api.Services;

public class OrderService(IRepository<Order, int> orderRepository, IMapper mapper)
{
    private readonly IRepository<Order, int> _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<OrderDto>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAsList();
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<List<OrderDto>> GetOrdersByPredicate(Func<Order, bool> predicate)
    {
        var orders = await _orderRepository.GetAsList(predicate);
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task AddOrder(OrderDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        await _orderRepository.Add(order);
    }

    public async Task UpdateOrder(OrderDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        await _orderRepository.Update(order);
    }

    public async Task DeleteOrder(int orderId)
    {
        await _orderRepository.Delete(orderId);
    }
}
