using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stock.Infrastructure.Common.Extensions;

public static class ConfigurationExtensions
{
    public static T GetOptions<T>(this IConfiguration configuration, bool required = false) where T : class
    {
        var bound = configuration.GetSection(typeof(T).Name).Get<T>();

        if (bound != null)
            Validator.ValidateObject(bound, new ValidationContext(bound), validateAllProperties: true);
        else if (required)
            throw new InvalidOperationException($"Settings type of '{nameof(T)}' was requested as required, but was not found in configuration.");

        return bound;
    }
    
    public static void RegisterOptions<T>(this IServiceCollection services) where T : class
    {
        services.AddSingleton<T>(serviceProvider =>
            serviceProvider.GetRequiredService<IConfiguration>().GetOptions<T>());
    }
}