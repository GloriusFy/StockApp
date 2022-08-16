using Stock.Domain.Common.ValueObjects.Mass;
using Stock.Domain.Common.ValueObjects.Money;

namespace Stock.Application.Common.Dependency.Services;

public interface IStockStatisticsService
{
    Task<Mass> GetProductStockTotalMass(MassUnit unit);
    Task<Money> GetProductStockTotalValue();
    Task<(int ProductCount, int TotalStock)> GetProductStockCounts();
}