using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Stock.Infrastructure;

namespace Stock.WebAPI;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplication
            .CreateBuilder(args)
            .CreateWebApplication<Startup>()
            .Run();

        
    }
    
    private static WebApplication CreateWebApplication<Startup>(this WebApplicationBuilder builder)
    {
        var startupType = typeof(Startup);
        var startupInstance = (IStockStartup)Activator.CreateInstance(startupType, new object[]
        {
            builder.Environment,
            builder.Configuration
        })!;

        
        
        startupInstance.ConfigureServices(builder.Services);

        builder.Host.ConfigureAppConfiguration((context, config) =>
        {
            config.AddUserSecrets(Assembly.GetEntryAssembly(), optional: true);
            config.AddInfrastructureConfiguration(context);
        });
        
        var application = builder.Build();
        
        startupInstance.Configure(application);

        // startupInstance.ConfigureMiddleware(application, application.Services);
        // startupInstance.ConfigureEndpoints(application, application.Services);
        
        return application;
    }
    
}

