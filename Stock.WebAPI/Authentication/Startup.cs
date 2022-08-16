using System.Diagnostics.CodeAnalysis;
using Stock.Application.Common.Dependency.Services;
using Stock.WebAPI.Authentication.Services;

namespace Stock.WebAPI.Authentication;

internal static class Startup
{
    internal static void AddApiAuth(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
    }
}