using System.Diagnostics.CodeAnalysis;
using Stock.Infrastructure.Common.Extensions;
using Stock.WebAPI.Cors.Settings;

namespace Stock.WebAPI.Cors;

internal static class Startup
{

    internal static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // var corsSettings = configuration.GetOptions<CorsSettings>();
        //
        // if (corsSettings == null)
        // {
        //     return;
        // }
        //
        services.AddCors();
        // services.AddCors(options =>
        // {
        //     options.AddDefaultPolicy(builder =>
        //     {
        //         builder
        //             .AllowAnyMethod()
        //             .AllowAnyHeader()
        //             .AllowCredentials()
        //             .SetIsOriginAllowedToAllowWildcardSubdomains()
        //             .WithOrigins(
        //                 corsSettings.AllowedOrigins)
        //             .Build();
        //     });
        // });
    }

    public static void UseCorsConfiguration(this IApplicationBuilder app)
    {
        app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
    }
}