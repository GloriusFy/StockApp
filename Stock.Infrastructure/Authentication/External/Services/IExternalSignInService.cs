using Stock.Infrastructure.Authentication.Core.Model;
using Stock.Infrastructure.Authentication.External.Models;

namespace Stock.Infrastructure.Authentication.External.Services;

public interface IExternalSignInService
{
    Task<(SignInResult result, SignInData? data)> SignInExternal(ExternalAuthenticationProvider provider, string idToken);
}