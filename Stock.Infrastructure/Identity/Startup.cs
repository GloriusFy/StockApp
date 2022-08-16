using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Stock.Infrastructure.Identity.Models;
using Stock.Infrastructure.Persistence.Context;

namespace Stock.Infrastructure.Identity;

internal static class Startup
{
    internal static void ConfigureServices(IServiceCollection services, IConfiguration _)
    {
        services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }
}