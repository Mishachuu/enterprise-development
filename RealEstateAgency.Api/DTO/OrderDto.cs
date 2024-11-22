using System.ComponentModel.DataAnnotations;
using static RealEstateAgency.Domain.Order;

namespace RealEstateAgency.Api.DTO;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public int ClientId { get; set; }
    public decimal Price { get; set; }

    [EnumDataType(typeof(PurchaseOrSale))]
    public string? Type { get; set; }
    public int RealEstateId { get; set; }
}
