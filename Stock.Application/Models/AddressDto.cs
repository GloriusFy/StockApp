namespace Stock.Application.Models;

#nullable disable
public sealed record AddressDto
{
    public string Country { get; init; }
    public string ZipCode { get; init; }
    public string Street { get; init; }
    public string City { get; init; }
}
#nullable restore