using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Stock.Infrastructure.Common.Extensions;
using Stock.Infrastructure.Persistence.Context;
using Stock.Infrastructure.Persistence.Settings;

namespace Stock.Infrastructure.Persistence;

internal static class Startup
{
    internal static void ConfigureServices(this IServiceCollection services, 
        IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton(configuration.GetOptions<ApplicationDbSettings>());

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetOptions<ConnectionStrings>().DefaultConnection,
                opt =>
                    opt.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            
            if (environment.IsDevelopment())
                options.EnableSensitiveDataLogging();
        });
    }
}