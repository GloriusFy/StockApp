using Stock.Infrastructure.Authentication.Core.Model;
using Stock.Infrastructure.Authentication.Core.Services;
using Stock.Infrastructure.Authentication.External.Exceptions;
using Stock.Infrastructure.Authentication.External.Models;

namespace Stock.Infrastructure.Authentication.External.Services;

internal class ExternalSignInService : IExternalSignInService
{
    private readonly ITokenService _tokenService;
    private readonly IExternalAuthenticationVerifier _verifier;

    public ExternalSignInService(IExternalAuthenticationVerifier verifier, ITokenService tokenService)
    {
        _verifier = verifier;
        _tokenService = tokenService;
    }

    public async Task<(SignInResult result, SignInData? data)> SignInExternal(ExternalAuthenticationProvider provider,
        string idToken)
    {
        var (success, userData) = await _verifier.Verify(provider, idToken);

        if (!success) return (SignInResult.Failed, null);

        if (string.IsNullOrWhiteSpace(userData.Email) || string.IsNullOrWhiteSpace(userData.FullName))
        {
            var missingFields = new List<string>();
            if (string.IsNullOrWhiteSpace(userData.Email)) missingFields.Add(nameof(ExternalUserData.Email));
            if (string.IsNullOrWhiteSpace(userData.FullName)) missingFields.Add(nameof(ExternalUserData.FullName));

            throw new ExternalAuthenticationInfoException(
                missingFields,
                userData
            );
        }


        var token = _tokenService.CreateAuthenticationToken(
            $"ext:{provider}:{userData.Email}",
            $"{userData.FullName} ({provider})");

        return (
            result: SignInResult.Success,
            data: new SignInData
            {
                ExternalAuthenticationProvider = provider.ToString(),
                Username = userData.FullName,
                Email = userData.Email,
                Token = token
            }
        );
    }
}