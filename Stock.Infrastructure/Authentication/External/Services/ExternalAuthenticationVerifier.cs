using System.ComponentModel;
using Google.Apis.Auth;
using Stock.Infrastructure.Authentication.External.Exceptions;
using Stock.Infrastructure.Authentication.External.Models;
using Stock.Infrastructure.Authentication.Settings;

namespace Stock.Infrastructure.Authentication.External.Services;

internal class ExternalAuthenticationVerifier : IExternalAuthenticationVerifier
{
    private readonly ExternalAuthenticationSettings _externalAuthSettings;

    public ExternalAuthenticationVerifier(ExternalAuthenticationSettings settings)
    {
        _externalAuthSettings = settings;
    }

    public Task<(bool success, ExternalUserData userData)> Verify(ExternalAuthenticationProvider provider,
        string idToken)
    {
        return provider switch
        {
            ExternalAuthenticationProvider.Google => AuthenticateGoogleToken(idToken),
            _ => throw new InvalidEnumArgumentException($"Support for provider '{provider}' is not implemented.")
        };
    }

    private async Task<(bool success, ExternalUserData userData)> AuthenticateGoogleToken(string idToken)
    {
        if (string.IsNullOrWhiteSpace(_externalAuthSettings.GoogleClientId))
            throw new ExternalAuthenticationSetupException("Google");

        GoogleJsonWebSignature.Payload data;
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _externalAuthSettings.GoogleClientId }
            };
            data = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
        catch (Exception ex)
        {
            if (ex is InvalidJwtException) return (false, null);

            throw new ExternalAuthenticationPreventedException(ex);
        }

        return (
            success: true,
            userData: new ExternalUserData
            (
                data.Email,
                data.EmailVerified,
                data.Name,
                LastName: data.FamilyName,
                FirstName: data.GivenName
            )
        );
    }
}