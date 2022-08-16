using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Stock.Infrastructure;

public static class Startup
{
    public static void AddInfrastructureConfiguration(this IConfigurationBuilder configurationBuilder,
        HostBuilderContext context)
    {
        configurationBuilder.AddJsonFile("InfrastructureSettings.json", true);
    }

    public static void AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        Identity.Startup.ConfigureServices(services, configuration);
        Authentication.Startup.ConfigureServices(services, configuration);
        Persistence.Startup.ConfigureServices(services, configuration, environment);
        ApplicationDependencies.Startup.ConfigureServices(services, configuration);
    }

    public static void UseInfrastructure(this IApplicationBuilder application, 
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        Authentication.Startup.Configure(application);
    }
    
    
}