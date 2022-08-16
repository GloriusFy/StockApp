namespace Stock.Infrastructure.Authentication.External.Models;

public record ExternalUserData(
    string Email,
    bool EmailVerified,
    string FullName,
    string FirstName,
    string LastName);
