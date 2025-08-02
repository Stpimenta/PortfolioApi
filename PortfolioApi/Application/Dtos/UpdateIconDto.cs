using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class UpdateIconDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Path { get; set; } = null!;
}