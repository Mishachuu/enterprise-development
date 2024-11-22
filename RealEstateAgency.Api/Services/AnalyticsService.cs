using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Interface;

namespace RealEstateAgency.Api.Services;

public class AnalyticsService(IRepository<Order, int> orderRepository, IRepository<Client, int> clientRepository, IMapper mapper)
{
    public async Task<List<ClientDto>> GetClientsByPropertyTypeAsync(RealEstate.PropertyType propertyType)
    {
        var orders = await orderRepository.GetAsList(o => o != null && o.RealEstate != null && o.RealEstate.Type == propertyType && o.Client != null);
        var clientIds = orders.Select(o => o.Client.Id).Distinct().ToList();

        var clients = (await clientRepository.GetAsList(c => clientIds.Contains(c.Id))).OrderBy(c => c.FirstAndLastName);

        return mapper.Map<List<ClientDto>>(clients.ToList());
    }


    public async Task<List<ClientDto>> GetSellersByPeriod(DateTime startDate, DateTime endDate)
    {
        var orders = await orderRepository.GetAsList(o => o.Type == Order.PurchaseOrSale.Sale && o.Time >= startDate && o.Time <= endDate && o.Client != null);
        var clientIds = orders.Select(o => o.Client.Id).Distinct().ToList();

        var clients = await clientRepository.GetAsList(c => clientIds.Contains(c.Id));

        return mapper.Map<List<ClientDto>>(clients);
    }

    public async Task<SellerRealEstateDto> GetSellersForBuyerOrder(int buyerOrderId)
    {
        var buyerOrders = await orderRepository.GetAsList();
        var buyerOrder = buyerOrders
    .FirstOrDefault(o => o.Id == buyerOrderId && o.RealEstate != null) ?? throw new ArgumentException("Заказ покупателя не найден.");

        var realEstate = buyerOrder.RealEstate;

        var sellerOrders = await orderRepository.GetAsList(o =>
            o.Type == Order.PurchaseOrSale.Sale &&
            o.RealEstate.Id == realEstate.Id &&
            o.Price == buyerOrder.Price);

        var sellerClientIds = sellerOrders
            .Select(o => o.Client.Id)
            .Distinct()
            .ToList();

        var matchingSellers = await clientRepository.GetAsList(c =>
            sellerClientIds.Contains(c.Id));

        var result = new SellerRealEstateDto
        {
            RealEstate = mapper.Map<RealEstateDto>(realEstate),
            Sellers = mapper.Map<List<ClientDto>>(matchingSellers)
        };

        return result;
    }


    public async Task<List<RealEstateOrderCountDto>> GetOrderCountByRealEstateType()
    {
        var orders = await orderRepository.GetAsList();
        if (orders.Count == 0) return [];

        return orders.Where(o => o?.RealEstate?.Type != null)
            .GroupBy(o => o.RealEstate.Type.ToString())
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
            .Where(o => o.Type == Order.PurchaseOrSale.Purchase && o.Client != null)
            .GroupBy(o => o.Client.Id)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new { ClientId = g.Key, OrderCount = g.Count() })
            .ToList();

        var clients = await clientRepository.GetAsList(c => topPurchasers.Select(tp => tp.ClientId).Contains(c.Id));

        return clients.Select(c => new ClientOrderCountDto
        {
            Client = mapper.Map<ClientDto>(c),
            OrderCount = topPurchasers.First(tp => tp.ClientId == c.Id).OrderCount
        }).ToList();
    }

    public async Task<List<ClientOrderCountDto>> GetTop5Sellers()
    {
        var orders = await orderRepository.GetAsList();

        var topSellers = orders
            .Where(o => o.Type == Order.PurchaseOrSale.Sale && o.Client != null)
            .GroupBy(o => o.Client.Id)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new { ClientId = g.Key, OrderCount = g.Count() })
            .ToList();

        var clients = await clientRepository.GetAsList(c => topSellers.Select(ts => ts.ClientId).Contains(c.Id));

        return clients.Select(c => new ClientOrderCountDto
        {
            Client = mapper.Map<ClientDto>(c),
            OrderCount = topSellers.First(ts => ts.ClientId == c.Id).OrderCount
        }).ToList();
    }

    public async Task<List<ClientOrderPriceDto>> GetClientsWithMinOrderPrice()
    {
        var orders = await orderRepository.GetAsList();
        if (orders.Count == 0) return [];

        var minPrice = orders.Min(o => o.Price);

        var minPriceOrders = orders.Where(o => o.Price == minPrice && o.Client != null).ToList();
        var clientIds = minPriceOrders.Select(o => o.Client.Id).Distinct().ToList();
        var clients = await clientRepository.GetAsList(c => clientIds.Contains(c.Id));

        var result = clients.Select(c => new ClientOrderPriceDto
        {
            Client = mapper.Map<ClientDto>(c),
            OrderPrice = minPriceOrders.First(o => o.Client.Id == c.Id).Price
        }).ToList();

        return result;
    }
}
