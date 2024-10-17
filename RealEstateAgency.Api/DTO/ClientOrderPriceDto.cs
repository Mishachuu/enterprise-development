namespace RealEstateAgency.Api.DTO;

public class ClientOrderPriceDto
{
    public ClientDto? Client { get; set; }
    public decimal OrderPrice { get; set; }
}