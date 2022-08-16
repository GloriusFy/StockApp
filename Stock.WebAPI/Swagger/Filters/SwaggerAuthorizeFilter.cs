using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Stock.WebAPI.Swagger.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stock.WebAPI.Swagger.Filters;

internal class SwaggerAuthorizeFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var filterDescriptor = context.ApiDescription.ActionDescriptor.FilterDescriptors;

        var hasAuthorizeFilter = filterDescriptor
            .Select(filterInfo => filterInfo.Filter)
            .Any(filter => filter is AuthorizeFilter);

        var allowAnonymous = filterDescriptor
            .Select(filterInfo => filterInfo.Filter)
            .Any(filter => filter is AllowAnonymousFilter);
        
        var hasAuthorizeAttribute = context.MethodInfo.DeclaringType
            .GetCustomAttributes(true)
            .Any(attribute => attribute is AuthorizeAttribute)
            || context.MethodInfo.GetCustomAttributes(true).Any(attribute => attribute is AuthorizeAttribute);

        if ((hasAuthorizeFilter || hasAuthorizeAttribute) && !allowAnonymous)
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security ??= new List<OpenApiSecurityRequirement>();
            
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SecuritySchemeNames.ApiLogin
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        }
    }
}