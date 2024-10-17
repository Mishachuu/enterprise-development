using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Api.Services;

public class AnalyticsService(IRepository<Order, int> orderRepository, IRepository<Client, int> clientRepository, IMapper mapper)
{

    public async Task<List<ClientDto>> GetClientsByRealEstateType(string type)
    {
        if (!Enum.TryParse<RealEstate.PropertyType>(type, true, out var propertyType))
            throw new ArgumentException("Неверный тип недвижимости.");

        var orders = await orderRepository.GetAsList(o => o != null && o.Type == Order.PurchaseOrSale.Purchase && o.Item.Type == propertyType);
        var clientIds = orders.Select(o => o.Client.ClientId).Distinct().ToList();

        var clients = await clientRepository.GetAsList(c => clientIds.Contains(c.ClientId));

        return clients.OrderBy(c => c.FirstAndLastName).Select(c => mapper.Map<ClientDto>(c)).ToList();
    }

    public async Task<List<ClientDto>> GetSellersByPeriod(DateTime startDate, DateTime endDate)
    {
        var orders = await orderRepository.GetAsList(o => o.Type == Order.PurchaseOrSale.Sale && o.Time >= startDate && o.Time <= endDate);
        var clientIds = orders.Select(o => o.Client.ClientId).Distinct().ToList();

        var clients = await clientRepository.GetAsList(c => clientIds.Contains(c.ClientId));

        return clients.Select(c => mapper.Map<ClientDto>(c)).ToList();
    }

    public async Task<SellerRealEstateDto> GetSellersForBuyerOrder(int buyerOrderId)
    {
        var buyerOrder = (await orderRepository.GetAsList(o => o.Id == buyerOrderId && o.Type == Order.PurchaseOrSale.Purchase))
                         .FirstOrDefault() ?? throw new ArgumentException("Заказ покупателя не найден.");

        var realEstate = buyerOrder.Item;

        var matchingSellers = await clientRepository.GetAsList(c =>
            orderRepository.GetAsList(o =>
                o.Type == Order.PurchaseOrSale.Sale &&
                o.Item.Id == realEstate.Id &&
                o.Price == buyerOrder.Price).Result.Any(o => o.Client.ClientId == c.ClientId)
        );

        var result = new SellerRealEstateDto
        {
            RealEstate = mapper.Map<RealEstateDto>(realEstate),
            Sellers = matchingSellers.Select(c => mapper.Map<ClientDto>(c)).ToList()
        };

        return result;
    }


    public async Task<List<RealEstateOrderCountDto>> GetOrderCountByRealEstateType()
    {
        var orders = await orderRepository.GetAsList();
        if (orders.Count == 0) return [];

        return orders.Where(o => o?.Item?.Type != null)
            .GroupBy(o => o.Item.Type.ToString())
            .Select(g => new RealEstateOrderCountDto
            {
                RealEstateType = g.Key,
                OrderCount = g.Count()
            })
            .ToList();
    }

    public async Task<List<ClientOrderCountDto>> GetTop5Purchasers()
    {
        var orders = await orderRepository.GetAsList();

        var topPurchasers = orders
            .Where(o => o.Type == Order.PurchaseOrSale.Purchase)
            .GroupBy(o => o.Client.ClientId)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new { ClientId = g.Key, OrderCount = g.Count() })
            .ToList();

        var clients = await clientRepository.GetAsList(c => topPurchasers.Select(tp => tp.ClientId).Contains(c.ClientId));

        return clients.Select(c => new ClientOrderCountDto
        {
            Client = mapper.Map<ClientDto>(c),
            OrderCount = topPurchasers.First(tp => tp.ClientId == c.ClientId).OrderCount
        }).ToList();
    }

    public async Task<List<ClientOrderCountDto>> GetTop5Sellers()
    {
        var orders = await orderRepository.GetAsList();

        var topSellers = orders
            .Where(o => o.Type == Order.PurchaseOrSale.Sale)
            .GroupBy(o => o.Client.ClientId)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new { ClientId = g.Key, OrderCount = g.Count() })
            .ToList();

        var clients = await clientRepository.GetAsList(c => topSellers.Select(ts => ts.ClientId).Contains(c.ClientId));

        return clients.Select(c => new ClientOrderCountDto
        {
            Client = mapper.Map<ClientDto>(c),
            OrderCount = topSellers.First(ts => ts.ClientId == c.ClientId).OrderCount
        }).ToList();
    }

    public async Task<List<ClientOrderPriceDto>> GetClientsWithMinOrderPrice()
    {
        var orders = await orderRepository.GetAsList();
        if (orders.Count == 0) return [];

        var minPrice = orders.Min(o => o.Price);

        var minPriceOrders = orders.Where(o => o.Price == minPrice).ToList();
        var clientIds = minPriceOrders.Select(o => o.Client.ClientId).Distinct().ToList();
        var clients = await clientRepository.GetAsList(c => clientIds.Contains(c.ClientId));

        var result = clients.Select(c => new ClientOrderPriceDto
        {
            Client = mapper.Map<ClientDto>(c),
            OrderPrice = minPriceOrders.First(o => o.Client.ClientId == c.ClientId).Price
        }).ToList();

        return result;
    }
}
