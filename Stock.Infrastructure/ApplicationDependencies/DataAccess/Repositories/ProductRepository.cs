using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stock.Application.Common.Dependency.DataAccess.Repositories;
using Stock.Domain.Products;
using Stock.Infrastructure.ApplicationDependencies.DataAccess.Repositories.Common;
using Stock.Infrastructure.Persistence.Context;

namespace Stock.Infrastructure.ApplicationDependencies.DataAccess.Repositories;

internal class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    protected override IQueryable<Product> BaseQuery
        => _context.Products.Include(x => x.Mass);

    public ProductRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    { }

    public Task<List<Product>> GetHeaviestProducts(int numberOfProducts)
        => BaseQuery
            .OrderByDescending(p => p.Mass)
            .Take(numberOfProducts)
            .ToListAsync();

    public Task<List<Product>> GetMostStockedProducts(int numberOfProducts)
        => BaseQuery
            .OrderByDescending(p => p.NumberInStock)
            .Take(numberOfProducts)
            .ToListAsync();

    public override void Remove(Product entityToDelete)
        => _context.Remove(entityToDelete);

    public override void RemoveRange(IEnumerable<Product> entitiesToDelete)
    {
        foreach (var e in entitiesToDelete)
            Remove(e);
    }
}