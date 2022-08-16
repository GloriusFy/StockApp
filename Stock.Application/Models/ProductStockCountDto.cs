namespace Stock.Application.Models;

public sealed record ProductStockCountDto
{
    public int ProductCount { get; init; }
    public int TotalStock { get; init; }
}