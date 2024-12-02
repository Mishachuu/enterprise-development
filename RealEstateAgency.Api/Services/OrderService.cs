using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;
using System.Linq.Expressions;


namespace RealEstateAgency.Api.Services;

public class OrderService(
    IRepository<Order, int> orderRepository,
    IRepository<Client, int> clientRepository,
    IRepository<RealEstate, int> realEstateRepository,
    IMapper mapper)
{

    public async Task<List<OrderGetDto>> GetAllOrders()
    {
        var orders = await orderRepository.GetAsList();
        return mapper.Map<List<OrderGetDto>>(orders);
    }

    public async Task<List<OrderGetDto>> GetOrdersByPredicate(Expression<Func<Order, bool>> predicate)
    {
        var orders = await orderRepository.GetAsList(predicate);
        return mapper.Map<List<OrderGetDto>>(orders);
    }

    public async Task AddOrder(OrderDto orderDto)
    {
        var order = mapper.Map<Order>(orderDto);

        var clientList = await clientRepository.GetAsList(c => c.Id == orderDto.ClientId);
        var client = clientList.FirstOrDefault() ?? throw new ArgumentException($"Клиент с ID {orderDto.ClientId} не существует.");
        var realEstateList = await realEstateRepository.GetAsList(r => r.Id == orderDto.RealEstateId);
        var realEstate = realEstateList.FirstOrDefault() ?? throw new ArgumentException($"Объект недвижимости с ID {orderDto.RealEstateId} не существует.");
        order.Client = client;
        order.RealEstate = realEstate;

        await orderRepository.Add(order);
    }

    public async Task UpdateOrder(int id, OrderDto orderDto)
    {
        var existingOrderList = await orderRepository.GetAsList(o => o.Id == id);
        var existingOrder = existingOrderList.FirstOrDefault() ?? throw new ArgumentException($"Заявка с ID {id} не существует.");
        var order = mapper.Map<Order>(orderDto);
        order.Id = id;

        var clientList = await clientRepository.GetAsList(c => c.Id == orderDto.ClientId);
        var client = clientList.FirstOrDefault() ?? throw new ArgumentException($"Клиент с ID {orderDto.ClientId} не существует.");
        order.Client = client;

        var realEstateList = await realEstateRepository.GetAsList(r => r.Id == orderDto.RealEstateId);
        var realEstate = realEstateList.FirstOrDefault() ?? throw new ArgumentException($"Объект недвижимости с ID {orderDto.RealEstateId} не существует.");
        order.RealEstate = realEstate;

        await orderRepository.Update(order);
    }

    public async Task DeleteOrder(int orderId)
    {
        await orderRepository.Delete(orderId);
    }
}
