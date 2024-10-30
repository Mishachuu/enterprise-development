using RealEstateAgency.Domain;

namespace RealEstateAgency.Tests;

public class RealEstateAgencyQueryTests
{
    private readonly List<Client> _testClients = TestData.Clients;
    private readonly List<Order> _testOrders = TestData.Orders;

    [Fact]
    public void GetClientsSearchingForSpecificRealEstateTypeShouldReturnOrderedByFullName()
    {
        var realEstateType = RealEstate.PropertyType.Residential;

        var clients = _testClients
            .Where(c => _testOrders.Any(o => o.Client == c && o.Type == Order.PurchaseOrSale.Purchase && o.RealEstate.Type == realEstateType))
            .OrderBy(c => c.FirstAndLastName)
            .ToList();

        Assert.NotEmpty(clients);
        Assert.Equal("Chukarev Michail", clients[0].FirstAndLastName);
    }

    [Fact]
    public void GetSellersWithinPeriodShouldReturnCorrectSellers()
    {
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 11, 1);

        var sellers = _testClients
            .Where(c => _testOrders.Any(o => o.Client == c && o.Type == Order.PurchaseOrSale.Sale && o.Time >= startDate && o.Time <= endDate))
            .ToList();

        Assert.NotEmpty(sellers);
        Assert.Contains(sellers, s => s.FirstAndLastName == "Chukarev Michail");
        Assert.Contains(sellers, s => s.FirstAndLastName == "Stepanov Dima");
    }

    [Fact]
    public void GetSellersForBuyerOrderShouldReturnSellersWithMatchingRealEstate()
    {
        var buyerOrder = _testOrders
            .First(o => o.Type == Order.PurchaseOrSale.Purchase && o.RealEstate.Type == RealEstate.PropertyType.Residential);

        var sellers = _testClients
            .Join(_testOrders.Where(o => o.Type == Order.PurchaseOrSale.Sale && o.RealEstate.Type == buyerOrder.RealEstate.Type && o.Price == buyerOrder.Price),
                  seller => seller.Id,
                  saleOrder => saleOrder.Client.Id,
                  (seller, saleOrder) => seller)
            .Distinct()
            .ToList();

        Assert.NotEmpty(sellers);
        Assert.Equal("Stepanov Dima", sellers[0].FirstAndLastName);
    }

    [Fact]
    public void GetOrderCountByRealEstateTypeShouldReturnCorrectCounts()
    {
        var orderCountByType = _testOrders
            .GroupBy(o => o.RealEstate.Type)
            .Select(g => new
            {
                RealEstateType = g.Key,
                OrderCount = g.Count()
            })
            .ToList();

        Assert.NotEmpty(orderCountByType);
        var residentialCount = orderCountByType.FirstOrDefault(t => t.RealEstateType == RealEstate.PropertyType.Residential);
        var commercialCount = orderCountByType.FirstOrDefault(t => t.RealEstateType == RealEstate.PropertyType.Uninhabitable);

        Assert.NotNull(residentialCount);
        Assert.NotNull(commercialCount);

        Assert.Equal(5, residentialCount.OrderCount);
        Assert.Equal(1, commercialCount.OrderCount);
    }

    [Fact]
    public void GetTop5ClientsByOrderCountShouldReturnTop5Clients()
    {
        var purchaseOrders = _testClients
            .OrderByDescending(c => _testOrders.Count(o => o.Client == c && o.Type == Order.PurchaseOrSale.Purchase))
            .Take(5)
            .ToList();

        var saleOrders = _testClients
            .OrderByDescending(c => _testOrders.Count(o => o.Client == c && o.Type == Order.PurchaseOrSale.Sale))
            .Take(5)
            .ToList();

        Assert.NotEmpty(purchaseOrders);
        Assert.True(purchaseOrders.Count <= 5);
        Assert.Equal("Chukarev Michail", purchaseOrders[0].FirstAndLastName);

        Assert.NotEmpty(saleOrders);
        Assert.True(saleOrders.Count <= 5);
        Assert.Equal("Chukarev Michail", saleOrders[0].FirstAndLastName);
    }

    [Fact]
    public void GetClientsWithMinOrderPriceShouldReturnClientsWithMinPrice()
    {
        var minPrice = _testOrders.Min(o => o.Price);

        var clientsWithMinPrice = _testClients
            .Where(c => _testOrders.Any(o => o.Client == c && o.Price == minPrice))
            .ToList();

        Assert.NotEmpty(clientsWithMinPrice);
        Assert.Equal(500000, minPrice);
        Assert.Contains(clientsWithMinPrice, c => c.FirstAndLastName == "Ivanov Ivan");
    }
}
