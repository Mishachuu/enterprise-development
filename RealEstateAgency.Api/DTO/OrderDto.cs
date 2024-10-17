namespace RealEstateAgency.Api.DTO;

public class OrderDto
{
    public DateTime Time { get; set; }
    public int ClientId { get; set; }
    public decimal Price { get; set; }
    public string? Type { get; set; }
    public int RealEstateId { get; set; }
}
