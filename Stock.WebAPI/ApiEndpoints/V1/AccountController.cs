using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stock.Infrastructure.Authentication.Core.Model;
using Stock.Infrastructure.Authentication.Core.Services;
using Stock.Infrastructure.Authentication.External.Services;
using Stock.Infrastructure.Identity.Models;
using Stock.WebAPI.Authentication.Models;
using SignInResult = Stock.Infrastructure.Authentication.Core.Model.SignInResult;

namespace Stock.WebAPI.ApiEndpoints.V1;

public class AccountController : ApiControllerBase
{
    private readonly IUserService _userService;
    private readonly IExternalSignInService _externalSignInService;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(IUserService userService, IExternalSignInService externalSignInService, UserManager<AppUser> userManager)
    {
        _userService = userService;
        _externalSignInService = externalSignInService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto login)
        => ProduceLoginResponse(
            await _userService.SignIn(login.Username, login.Password));
    
    [AllowAnonymous]
    [HttpPost("loginExternal")]
    public async Task<ActionResult<LoginResponseDto>> ExternalLogin(ExternalLoginDto login)
        => ProduceLoginResponse(
            await _externalSignInService.SignInExternal(login.Provider, login.IdToken));
    
    private ActionResult<LoginResponseDto> ProduceLoginResponse((SignInResult result, SignInData data) loginResults)
    {
        var (result, data) = loginResults;

        return result switch
        {
            SignInResult.Failed => Unauthorized("Username or password incorrect."),
            SignInResult.Success => Ok(new LoginResponseDto()
            {
                AccessToken = data.Token.AccessToken,
                TokenType = data.Token.TokenType,
                ExpiresIn = data.Token.GetRemainingLifetimeSeconds(),
                Username = data.Username,
                Email = data.Email,
                IsExternalLogin = data.IsExternalLogin,
                ExternalAuthenticationProvider = data.ExternalAuthenticationProvider
            }),
            _ => throw new InvalidEnumArgumentException($"Unknown sign-in result '{result}'.")
        };
    }
}