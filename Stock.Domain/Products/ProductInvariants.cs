using Stock.Domain.Common.ValueObjects.Mass;
using Stock.Domain.Common.ValueObjects.Money;

namespace Stock.Domain.Products;

public static class ProductInvariants
{
    public const int NameMaxLength = 100;
    public const int DescriptionMaxLength = 1000;

    public const float MassMinimum = 0.1f;
    public const decimal PriceMinimum = 0.1m;

    public static readonly MassUnit DefaultMassUnit = MassUnit.Kilogram;
    public static readonly Currency DefaultPriceCurrency = Currency.RUB;
}