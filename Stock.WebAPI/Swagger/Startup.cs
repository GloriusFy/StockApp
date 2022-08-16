using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Stock.Infrastructure.Common.Extensions;
using Stock.WebAPI.Swagger.Configuration;
using Stock.WebAPI.Swagger.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Stock.WebAPI.Swagger;

internal static class Startup
{
    internal static void AddCustomizedSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetOptions<SwaggerSettings>();

        if (swaggerSettings.UseSwagger == false || swaggerSettings == null)
        {
            return;
        }

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = swaggerSettings.ApiName, Version = "v1"
            });
            
            options.AddSecurityDefinition(SecuritySchemeNames.ApiLogin, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(swaggerSettings.LoginPath, UriKind.Relative)
                    }
                }
            });
            
            options.CustomSchemaIds(type =>
            {
                var lastNamespaceSection = type.Namespace[(type.Name.LastIndexOf('.') + 1)..];
                var genericParameters = string.Join(',', (IEnumerable<Type>)type.GetGenericArguments());
                
                return $"{lastNamespaceSection}.{type.Name}" +
                       $"{(string.IsNullOrEmpty(genericParameters) ? null : "<" + genericParameters + ">")}";
            });
            
            options.OperationFilter<SwaggerGroupFilter>();
            options.OperationFilter<SwaggerAuthorizeFilter>();
        });
        
        services.AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUIOptions>();
    }

    internal static void UseCustomizedSwagger(this IApplicationBuilder application, IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetOptions<SwaggerSettings>();

        if (swaggerSettings.UseSwagger == true)
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }
    }
}