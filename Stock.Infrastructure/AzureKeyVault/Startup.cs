using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Stock.Infrastructure.AzureKeyVault.Settings;
using Stock.Infrastructure.Common.Extensions;

namespace Stock.Infrastructure.AzureKeyVault;

internal static class Startup
{
    internal static void ConfigureAppConfiguration(HostBuilderContext _, IConfigurationBuilder configurationBuilder)
    {
        var settings = configurationBuilder.Build().GetOptions<AzureKeyVaultSettings>();

        if (settings.AddToConfiguration)
        {
            configurationBuilder.AddAzureKeyVault(
                settings.ServiceUrl,
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                        new AzureServiceTokenProvider().KeyVaultTokenCallback)),
                new DefaultKeyVaultSecretManager()
            );
        }
    }
}