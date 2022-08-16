using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Stock.WebAPI.ErrorHandling.Filters;

namespace Stock.WebAPI.ErrorHandling;

internal static class Startup
{
    public static void AddErrorHandling(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(o =>
        {
            if (o == null)
            {
                throw new ArgumentException($"Cannot find {nameof(MvcOptions)}. " +
                                            "This module depends on MVC being already added, via e.g. AddControllers().");
            }

            o.Filters.Add<ExceptionMappingFilter>();
        });
    }
}