using Microsoft.AspNetCore.Identity;
using Stock.Infrastructure.Authentication.Core.Model;
using Stock.Infrastructure.Identity.Models;
using SignInResult = Stock.Infrastructure.Authentication.Core.Model.SignInResult;

namespace Stock.Infrastructure.Authentication.Core.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public UserService(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<(SignInResult result, SignInData? data)> SignIn(string username, string password)
    {
       var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return (SignInResult.Failed, null);
        }
        
        var result = await _userManager.CheckPasswordAsync(user, password);

        if (!result)
        {
            throw new System.Exception("Unhandled sign-in outcome.");
        }

        var token = _tokenService.CreateAuthenticationToken(user.Id, username);

        return (
            SignInResult.Success,
            data: new SignInData()
            {
                Username = user.UserName,
                Email = user.Email,
                Token = token
            }
        );
    }
    
}