using Stock.Infrastructure.Authentication.Core.Model;

namespace Stock.Infrastructure.Authentication.Core.Services;

public interface ITokenService
{
    TokenModel CreateAuthenticationToken(string userId, string uniqueName, IEnumerable<(string claimType, string claimValue)> customClaims = null);
}