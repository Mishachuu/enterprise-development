using static RealEstateAgency.Domain.Order;
namespace RealEstateAgency.Api.DTO;

public class OrderDto
{
    public int OrderId { get; set; }
    public DateTime Time { get; set; }
    public int ClientId { get; set; }
    public decimal Price { get; set; }
    public PurchaseOrSale Type { get; set; }
    public int RealEstateId { get; set; }
}
