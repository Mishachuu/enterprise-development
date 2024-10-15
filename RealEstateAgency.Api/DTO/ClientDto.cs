namespace RealEstateAgency.Api.DTO;

public class ClientDto
{
    public int ClientId { get; set; }
    public string FirstAndLastName { get; set; }
    public string Pasport { get; set; }
    public string? NumberPhone { get; set; }
    public string Address { get; set; }
    public string? Email { get; set; }
}
