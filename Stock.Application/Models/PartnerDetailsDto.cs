using AutoMapper;
using Stock.Application.Common.Mapping;
using Stock.Domain.Partners;

namespace Stock.Application.Models;

#nullable disable
public sealed record PartnerDetailsDto : IMapFrom<Partner>
{
    public string Id { get; init; }
    public string Name { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastModifiedAt { get; init; }

    public AddressDto Address { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Partner, PartnerDetailsDto>();
        profile.CreateMap<Address, AddressDto>();
    }
}
#nullable restore