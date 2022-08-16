using MediatR;
using Stock.Application.Common.Dependency.Services;
using Stock.Application.Models;

namespace Stock.Application.Products.ProductStockCount;

public sealed record ProductStockCountQuery : IRequest<ProductStockCountDto>;

public class ProductStockCountQueryHandler : IRequestHandler<ProductStockCountQuery, ProductStockCountDto>
{
    private readonly IStockStatisticsService _statisticsService;

    public ProductStockCountQueryHandler(IStockStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    public async Task<ProductStockCountDto> Handle(ProductStockCountQuery request, CancellationToken cancellationToken)
    {
        var result = await _statisticsService.GetProductStockCounts();

        return new ProductStockCountDto
        {
            ProductCount = result.ProductCount,
            TotalStock = result.TotalStock
        };
    }
}