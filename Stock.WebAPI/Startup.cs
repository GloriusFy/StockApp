using Stock.Application;
using Stock.Infrastructure;
using Stock.WebAPI.ApiEndpoints;
using Stock.WebAPI.Authentication;
using Stock.WebAPI.Cors;
using Stock.WebAPI.ErrorHandling;
using Stock.WebAPI.Swagger;

namespace Stock.WebAPI;

internal class Startup : IStockStartup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    
    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _configuration = configuration;
    }
    
    public void Configure(IApplicationBuilder application)
    {
        application.UseCorsConfiguration();
        
        if (_environment.IsDevelopment())
        {
            application.UseDeveloperExceptionPage();
        }

        application.UseHttpsRedirection();
        application.UseRouting();
        application.UseCustomizedSwagger(_configuration);
        application.UseInfrastructure(_configuration, _environment);
        application.UseApiEndpoints();

    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiEndpoints();
        services.AddApiAuth();
        services.AddErrorHandling();
        services.AddCustomizedSwagger(_configuration);
        services.AddApiVersioning();
        services.AddCorsConfiguration(_configuration);
        services.AddInfrastructureDependencies(_configuration, _environment);
        services.AddApplicationDependencies();
    }
}