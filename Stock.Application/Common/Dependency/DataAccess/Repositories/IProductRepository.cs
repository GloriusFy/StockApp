using Stock.Application.Common.Dependency.DataAccess.Repositories.Common;
using Stock.Domain.Products;

namespace Stock.Application.Common.Dependency.DataAccess.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetHeaviestProducts(int numberOfProducts);
    Task<List<Product>> GetMostStockedProducts(int numberOfProducts);
}