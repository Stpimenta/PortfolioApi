namespace PortfolioApi.Application.Dtos;

public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
