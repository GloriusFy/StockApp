using Stock.Infrastructure.Common.Validation;

namespace Stock.Infrastructure.AzureKeyVault.Settings;

internal class AzureKeyVaultSettings
{
    [RequiredIf(nameof(AddToConfiguration), true)]
    public string? ServiceUrl { get; init; }
    public bool AddToConfiguration { get; init; }
}