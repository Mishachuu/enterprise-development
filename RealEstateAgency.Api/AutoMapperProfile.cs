using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;

namespace RealEstateAgency.Api;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Client, ClientDto>().ReverseMap();

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Client.ClientId))
            .ForMember(dest => dest.RealEstateId, opt => opt.MapFrom(src => src.Item.Id))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ReverseMap()
            .ForMember(dest => dest.Client, opt => opt.Ignore())
            .ForMember(dest => dest.Item, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ParseOrderType(src.Type)));

        CreateMap<RealEstate, RealEstateDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ParsePropertyType(src.Type)));
    }

    private static Order.PurchaseOrSale ParseOrderType(string? type)
    {
        if (Enum.TryParse<Order.PurchaseOrSale>(type, true, out var result))
        {
            return result;
        }
        throw new ArgumentException($"Недопустимое значение OrderType: {type}");
    }

    private static RealEstate.PropertyType ParsePropertyType(string type)
    {
        if (Enum.TryParse<RealEstate.PropertyType>(type, true, out var result))
        {
            return result;
        }
        throw new ArgumentException($"Недопустимое значение PropertyType: {type}");
    }
}