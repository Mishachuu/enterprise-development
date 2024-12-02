namespace RealEstateAgency.WebApplication.Api;

public class RealEstateAgencyApiWrapper(IConfiguration configuration) : IRealEstateAgencyApiWrapper
{
    public readonly RealEstateAgencyApi _client = new(configuration["OpenApi:ServerUrl"], new HttpClient());

    public async Task CreateClient(ClientDto newClient) => await _client.ClientsPOSTAsync(newClient);
    public async Task CreateOrder(OrderDto newOrder) => await _client.OrdersPOSTAsync(newOrder);
    public async Task CreateRealEstates(RealEstateDto newRealEstate) => await _client.RealEstatesPOSTAsync(newRealEstate);

    public async Task UpdateClient(int id, ClientDto newClient) => await _client.ClientsPUTAsync(id, newClient);
    public async Task UpdateOrder(int id, OrderDto newOrder) => await _client.OrdersPUTAsync(id, newOrder);
    public async Task UpdateRealEstate(int id, RealEstateDto newRealEstate) => await _client.RealEstatesPUTAsync(id, newRealEstate);

    public async Task DeleteClient(int id) => await _client.ClientsDELETEAsync(id);
    public async Task DeleteOrder(int id) => await _client.OrdersDELETEAsync(id);
    public async Task DeleteRealEstate(int id) => await _client.RealEstatesDELETEAsync(id);

    public async Task<ClientGetDto> GetClient(int id) => await _client.ClientsGETAsync(id);
    public async Task<OrderGetDto> GetOrder(int id) => await _client.OrdersGETAsync(id);
    public async Task<RealEstateGetDto> GetRealEstate(int id) => await _client.RealEstatesGETAsync(id);

    public async Task<IList<ClientGetDto>> GetClients() => [.. await _client.ClientsAllAsync()];
    public async Task<IList<OrderGetDto>> GetOrder() => [.. await _client.OrdersAllAsync()];
    public async Task<IList<RealEstateGetDto>> GetRealEstate() => [.. await _client.RealEstatesAllAsync()];

    public async Task<IList<ClientDto>> ClientsByRealestateTypeAsync(string type) => [.. await _client.ClientsByRealestateTypeAsync(type)];
    public async Task<IList<ClientDto>> SellersByPeriodAsync(DateTime startDate, DateTime endData) => [.. await _client.SellersByPeriodAsync(startDate, endData)];
    public async Task<SellerRealEstateDto> MatchingSellersForBuyerAsync(int id) => await _client.MatchingSellersForBuyerAsync(id);
    public async Task<IList<RealEstateOrderCountDto>> OrderCountByTypeAsync() => [.. await _client.OrderCountByTypeAsync()];
    public async Task<IList<ClientOrderCountDto>> TopPurchasersAsync() => [.. await _client.TopPurchasersAsync()];
    public async Task<IList<ClientOrderCountDto>> TopSellersAsync() => [.. await _client.TopSellersAsync()];
    public async Task<IList<ClientOrderPriceDto>> MinPriceOrdersAsync() => [.. await _client.MinPriceOrdersAsync()];

}
