namespace Stock.WebAPI.Authentication.Models;

public class LoginResponseDto
{
    public string AccessToken { get; set; }

    // TODO: Consider supporting refresh tokens.
    //public string refresh_token { get; set; }

    public string TokenType { get; set; } = "Bearer";

    public int ExpiresIn { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string ExternalAuthenticationProvider { get; set; }

    public bool IsExternalLogin { get; set; }
}