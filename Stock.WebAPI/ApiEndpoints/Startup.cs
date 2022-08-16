using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Stock.WebAPI.ApiEndpoints;

internal static class Startup
{
    public static void AddApiEndpoints(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddControllers()
            .AddControllersAsServices()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddJsonOptions(c =>
                c.JsonSerializerOptions.PropertyNamingPolicy
                    = JsonNamingPolicy.CamelCase); // Supposed to be default, but just to make sure.
    }

    public static void UseApiEndpoints(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
    }
}