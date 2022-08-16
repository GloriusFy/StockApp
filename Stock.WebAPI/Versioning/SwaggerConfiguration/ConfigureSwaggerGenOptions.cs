using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stock.WebAPI.Versioning.SwaggerConfiguration;

internal class ConfigureSwaggerGenOptions : IPostConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _versionProvider;

    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider versionProvider)
    {
        _versionProvider = versionProvider;
    }


    public void PostConfigure(string name, SwaggerGenOptions options)
    {
        options.SwaggerGeneratorOptions.SwaggerDocs.Clear();

        foreach (var description in  _versionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo()
                {
                    Title = $"{nameof(Stock)} {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                });
        }
    }
}