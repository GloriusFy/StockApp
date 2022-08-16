using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Stock.Application.Common.Dependency.Services;

namespace Stock.WebAPI.Authentication.Services;

internal class CurrentUserService : ICurrentUserService
{
    private const string DefaultNonUserMoniker = "System";
    private const string UnknownUserMoniker = "Anonymous";
    
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            UserId = DefaultNonUserMoniker;
        }
        else
        {
            UserId = httpContextAccessor.HttpContext.User?.FindFirstValue(JwtRegisteredClaimNames.UniqueName)
                     ?? UnknownUserMoniker;
        }
    }

    public string UserId { get; }
}