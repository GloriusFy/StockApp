using MediatR;
using Stock.Application.Common.Dependency.Services;
using Stock.Application.Models;

namespace Stock.Application.Products.ProductStockValue;

public sealed record ProductStockValueQuery : IRequest<StockValueDto>;

public class ProductStockValueQueryHandler : IRequestHandler<ProductStockValueQuery, StockValueDto>
{
    private readonly IStockStatisticsService _stockStatistics;

    public ProductStockValueQueryHandler(IStockStatisticsService stockStatistics)
    {
        _stockStatistics = stockStatistics;
    }

    public async Task<StockValueDto> Handle(ProductStockValueQuery request, CancellationToken cancellationToken)
    {
        var totalStockValue = await _stockStatistics.GetProductStockTotalValue();

        return new StockValueDto{
            Amount =  totalStockValue.Amount,
            CurrencyCode = totalStockValue.Currency.Code
        };
    }
}