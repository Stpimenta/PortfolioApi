using Microsoft.AspNetCore.Identity;

namespace PortfolioApi.Application.Services;

public class PasswordService: IPasswordService
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string HashPassword(string plainPassword)
    {
        return _passwordHasher.HashPassword(null, plainPassword);
    }

    public bool VerifyPassword(string hashedPassword, string inputPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, inputPassword);
        return result == PasswordVerificationResult.Success;
    }
}