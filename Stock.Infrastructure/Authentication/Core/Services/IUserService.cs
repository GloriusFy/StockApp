using Stock.Infrastructure.Authentication.Core.Model;

namespace Stock.Infrastructure.Authentication.Core.Services;

public interface IUserService
{
    Task<(SignInResult result, SignInData? data)> SignIn(string username, string password);
}