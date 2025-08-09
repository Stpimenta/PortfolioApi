using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class UpdateIconDto
{
    [Required]
    public string? Name { get; set; } 

    public IFormFile? Icon { get; set; } 
}