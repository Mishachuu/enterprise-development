using AutoMapper;
using RealEstateAgency.Api.DTO;
using RealEstateAgency.Domain;

namespace RealEstateAgency.Api;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Client, ClientGetDto>();
        CreateMap<Client, ClientDto>().ReverseMap();

        CreateMap<Order, OrderGetDto>();
        CreateMap<Order, OrderGetDto>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Client.Id))
                .ForMember(dest => dest.RealEstateId, opt => opt.MapFrom(src => src.RealEstate.Id))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Client.Id))
            .ForMember(dest => dest.RealEstateId, opt => opt.MapFrom(src => src.RealEstate.Id))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ReverseMap()
            .ForMember(dest => dest.Client, opt => opt.Ignore())
            .ForMember(dest => dest.RealEstate, opt => opt.Ignore());

        CreateMap<RealEstate, RealEstateGetDto>();
        CreateMap<RealEstate, RealEstateDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ReverseMap();
    }
}