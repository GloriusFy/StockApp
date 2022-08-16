using Stock.Application.Common.Mapping;
using Stock.Domain.Products;

namespace Stock.Application.Models;

#nullable disable
public sealed record ProductDto : IMapFrom<Product>
{
    public string Id { get; init; }

    public string Name { get; init; }
    public string Description { get; init; }

    public decimal PriceAmount { get; init; }
    public string PriceCurrencyCode { get; init; }

    public float MassValue { get; init; }
    public string MassUnitSymbol { get; init; }

    public int NumberInStock { get; init; }
}
#nullable restore