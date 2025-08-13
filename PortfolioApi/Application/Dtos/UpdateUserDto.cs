using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class UpdateUserDto
{
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
        
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }
    public IFormFile? Config { get; set; }
        
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [RegularExpression(@"^(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\-]).+$", ErrorMessage = "Password must contain at least one special character.")]
    public string? Password { get; set; }  // opcional no update
}