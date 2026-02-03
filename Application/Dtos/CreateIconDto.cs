using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class CreateIconDto
{
    [Required]
    public string? Name { get; set; } 

    [Required]
    public IFormFile? Icon { get; set; } 
}