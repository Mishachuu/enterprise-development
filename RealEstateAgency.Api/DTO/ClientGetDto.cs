namespace RealEstateAgency.Api.DTO;

public class ClientGetDto
{
    public int Id { get; set; }
    public required string FirstAndLastName { get; set; }
    public required string Pasport { get; set; }
    public string? NumberPhone { get; set; }
    public required string Address { get; set; }
    public string? Email { get; set; }
}
