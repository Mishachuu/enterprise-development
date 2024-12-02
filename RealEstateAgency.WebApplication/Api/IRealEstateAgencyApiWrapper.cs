
namespace RealEstateAgency.WebApplication.Api;

public interface IRealEstateAgencyApiWrapper
{
    Task<IList<ClientDto>> ClientsByRealestateTypeAsync(string type);
    Task CreateClient(ClientDto newClient);
    Task CreateOrder(OrderDto newOrder);
    Task CreateRealEstates(RealEstateDto newRealEstate);
    Task DeleteClient(int id);
    Task DeleteOrder(int id);
    Task DeleteRealEstate(int id);
    Task<ClientGetDto> GetClient(int id);
    Task<IList<ClientGetDto>> GetClients();
    Task<IList<OrderGetDto>> GetOrder();
    Task<OrderGetDto> GetOrder(int id);
    Task<IList<RealEstateGetDto>> GetRealEstate();
    Task<RealEstateGetDto> GetRealEstate(int id);
    Task<SellerRealEstateDto> MatchingSellersForBuyerAsync(int id);
    Task<IList<ClientOrderPriceDto>> MinPriceOrdersAsync();
    Task<IList<RealEstateOrderCountDto>> OrderCountByTypeAsync();
    Task<IList<ClientDto>> SellersByPeriodAsync(DateTime startDate, DateTime endData);
    Task<IList<ClientOrderCountDto>> TopPurchasersAsync();
    Task<IList<ClientOrderCountDto>> TopSellersAsync();
    Task UpdateClient(int id, ClientDto newClient);
    Task UpdateOrder(int id, OrderDto newOrder);
    Task UpdateRealEstate(int id, RealEstateDto newRealEstate);
}