namespace Stock.Domain.Common.ValueObjects.Mass;

#pragma warning disable CS8618
public class MassUnit
{
    public string Name { get; init; }
    public string Symbol { get; init; }
    public float ConversionRateToGram { get; set; }

    private MassUnit()
    { }
    
    public static readonly MassUnit Tonne = new MassUnit()
    {
        Name = "tonne", Symbol = "t", ConversionRateToGram = 1000000f
    };
    
    public static readonly MassUnit Kilogram = new MassUnit()
    {
        Name = "kilogram", Symbol = "kg", ConversionRateToGram = 1000f
    };
    
    public static readonly MassUnit Gram = new MassUnit()
    {
        Name = "gram", Symbol = "g", ConversionRateToGram = 1f
    };

    public static MassUnit FromSymbol(string unitSymbol)
        => unitSymbol.ToLower() switch
        {
            "t" => Tonne,
            "kg" => Kilogram,
            "g" => Gram,
            _ => throw new ArgumentException("Unknown " +
                    $"{nameof(MassUnit)} value '{unitSymbol}'.", nameof(unitSymbol))
        };
}