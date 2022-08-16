namespace Stock.Application.Models;

#nullable disable
public record StockMassDto
{
    public float Value { get; init; }
    public string Unit { get; init; }
}
#nullable restore