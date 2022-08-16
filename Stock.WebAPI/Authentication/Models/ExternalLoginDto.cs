using Stock.Infrastructure.Authentication.External.Models;

namespace Stock.WebAPI.Authentication.Models;

public record ExternalLoginDto(
    ExternalAuthenticationProvider Provider,
    string IdToken);