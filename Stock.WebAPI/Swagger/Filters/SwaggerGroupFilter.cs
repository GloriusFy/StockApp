using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stock.WebAPI.Swagger.Filters;

internal class SwaggerGroupFilter : IOperationFilter 
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var customGroupAttribute = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<SwaggerGroupAttribute>()?
            .FirstOrDefault();

        if (customGroupAttribute != null && !string.IsNullOrWhiteSpace(customGroupAttribute.GroupName))
        {
            operation.Tags = new List<OpenApiTag>
            {
                new OpenApiTag
                {
                    Name = customGroupAttribute.GroupName
                }
            };
        }
    }
}