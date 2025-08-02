using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class CreateUserDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [RegularExpression(@"^(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\-]).+$", ErrorMessage = "Password must contain at least one special character.")]
    public string? Password { get; set; }
}