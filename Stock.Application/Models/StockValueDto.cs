namespace Stock.Application.Models;

#nullable disable
public sealed record StockValueDto
{
    public decimal Amount { get; init; }
    public string CurrencyCode { get; init; }
}
#nullable restore