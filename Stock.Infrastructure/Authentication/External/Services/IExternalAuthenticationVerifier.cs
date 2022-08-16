using Stock.Infrastructure.Authentication.External.Models;

namespace Stock.Infrastructure.Authentication.External.Services;

public interface IExternalAuthenticationVerifier
{
    Task<(bool success, ExternalUserData userData)> Verify(ExternalAuthenticationProvider provider, string idToken);
}