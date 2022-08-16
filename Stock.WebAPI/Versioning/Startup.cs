using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Stock.WebAPI.Versioning.SwaggerConfiguration;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Stock.WebAPI.Versioning;

internal static class Startup
{
    internal static void AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
        });

        if (services.Any(x => x.ServiceType == typeof(ISwaggerProvider)))
        {
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'y'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AddApiVersionParametersWhenVersionNeutral = true;
            });

            services.AddTransient<IPostConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddTransient<IPostConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUIOptions>();
        }
    }
}