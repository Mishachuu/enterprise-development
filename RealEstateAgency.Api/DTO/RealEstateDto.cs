using System.ComponentModel.DataAnnotations;
using static RealEstateAgency.Domain.RealEstate;

namespace RealEstateAgency.Api.DTO;

public class RealEstateDto
{
    public int Id { get; set; }
    public required string Address { get; set; }
    public double Square { get; set; }
    public int NumberOfRooms { get; set; }
    [EnumDataType(typeof(PropertyType))]
    public required string Type { get; set; }
}
