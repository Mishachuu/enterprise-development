namespace RealEstateAgency.Api.DTO;

public class RealEstateDto
{
    public required string Address { get; set; }
    public double Square { get; set; }
    public int NumberOfRooms { get; set; }
    public required string Type { get; set; }
}
