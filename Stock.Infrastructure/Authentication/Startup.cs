using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Stock.Infrastructure.Authentication.Core.Services;
using Stock.Infrastructure.Authentication.External.Services;
using Stock.Infrastructure.Authentication.Settings;
using Stock.Infrastructure.Common.Extensions;

namespace Stock.Infrastructure.Authentication;

internal static class Startup
{
    internal static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.RegisterOptions<AuthenticationSettings>();
        ConfigureLocalJwtAuthentication(services, configuration.GetOptions<AuthenticationSettings>());

        services.RegisterOptions<ExternalAuthenticationSettings>();
        services.AddScoped<IExternalAuthenticationVerifier, ExternalAuthenticationVerifier>();
        services.AddScoped<IExternalSignInService, ExternalSignInService>();
    }

    internal static void Configure(IApplicationBuilder application)
    {
        application.UseAuthentication();
        application.UseAuthorization();
    }

    private static void ConfigureLocalJwtAuthentication(IServiceCollection services,
        AuthenticationSettings authenticationSettings)
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
#if DEBUG
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = ctx => Task.FromResult(true)
                    };
#endif
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidIssuer = authenticationSettings.JwtIssuer,
                        ValidateAudience = false,
                        ValidAudience = authenticationSettings.JwtIssuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(authenticationSettings.JwtSigningKey),
                        ClockSkew = TimeSpan.FromMinutes(5),
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                    };
                });
    }
}