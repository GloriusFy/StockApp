using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Stock.Infrastructure.Common.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Stock.WebAPI.Swagger.Configuration;

internal class ConfigureSwaggerUIOptions : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IConfiguration _configuration;

    public ConfigureSwaggerUIOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(SwaggerUIOptions options)
    {
        var swaggerSettings = _configuration.GetOptions<SwaggerSettings>();
        
        options.SwaggerEndpoint(
            url: "/swagger/v1/swagger.json",
            name: $"{swaggerSettings.ApiName}v1");
    }
}