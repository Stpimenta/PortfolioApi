namespace PortfolioApi.Application.Services;

public interface IPasswordService
{
    string HashPassword(string plainPassword);
    bool VerifyPassword(string hashedPassword, string inputPassword);
}
