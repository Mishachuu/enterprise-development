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
            .ForMember(dest => dest.Item, opt => opt.Ignore());

        CreateMap<RealEstate, RealEstateDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ReverseMap();
    }
}