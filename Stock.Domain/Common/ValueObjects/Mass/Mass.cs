using System.ComponentModel.DataAnnotations;

namespace Stock.Domain.Common.ValueObjects.Mass;

#pragma warning disable CS8618
public class Mass
{
    public float Value { get; init; }

    [Required]
    public MassUnit Unit { get; init; }

    private Mass()
    { }

    public Mass(float value, MassUnit unit)
    {
        if (value < 0)
        {
            throw new ArgumentException("Value cannot be negative", nameof(value));
        }

        Value = value;
        Unit = unit;
    }

    public Mass ConvertTo(MassUnit newUnit)
    {
        if (newUnit == Unit)
        {
            return this;
        }

        return new Mass()
        {
            Value = Value * Unit.ConversionRateToGram / newUnit.ConversionRateToGram,
            Unit = newUnit
        };
    }

    public override string ToString()
        => $"{Value:n} {Unit.Symbol}";
}