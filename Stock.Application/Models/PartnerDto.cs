using AutoMapper;
using Stock.Application.Common.Mapping;
using Stock.Domain.Partners;

namespace Stock.Application.Models;

#nullable disable
public sealed record PartnerDto : IMapFrom<Partner>
{
    public string Id { get; init; }
    public string Name { get; init; }

    public string Address { get; init; }

    public string Country { get; init; }
    public string ZipCode { get; init; }
    public string City { get; init; }
    public string Street { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Partner, PartnerDto>()
            .ForMember(dest => dest.Country, x => x.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.ZipCode, x => x.MapFrom(src => src.Address.ZipCode))
            .ForMember(dest => dest.City, x => x.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, x => x.MapFrom(src => src.Address.Street));
    }
}
#nullable restore