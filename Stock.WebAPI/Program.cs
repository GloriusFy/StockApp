// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
//
// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseHttpsRedirection();
//
// app.UseAuthorization();
//
// app.MapControllers();
//
// app.Run();


using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Stock.Infrastructure;

namespace Stock.WebAPI;

[ExcludeFromCodeCoverage]
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

