using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Dependency.DataAccess.Repositories;
using Stock.Application.Common.Dependency.Services;
using Stock.Infrastructure.ApplicationDependencies.DataAccess;
using Stock.Infrastructure.ApplicationDependencies.DataAccess.Repositories;
using Stock.Infrastructure.ApplicationDependencies.Services;

namespace Stock.Infrastructure.ApplicationDependencies;

internal static class Startup
{
    internal static void ConfigureServices(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IStockStatisticsService, StockStatisticsService>();
    }
}