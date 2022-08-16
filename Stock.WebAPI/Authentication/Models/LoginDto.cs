namespace Stock.WebAPI.Authentication.Models;

public record LoginDto(
    string Username,
    string Password);