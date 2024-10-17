namespace RealEstateAgency.Api.DTO;

public class SellerRealEstateDto
{
    public RealEstateDto? RealEstate { get; set; }
    public List<ClientDto>? Sellers { get; set; }
}