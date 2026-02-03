using System.ComponentModel.DataAnnotations;

namespace PortfolioApi.Application.Dtos;

public class UpdateTechnologyDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public int? IconId { get; set; }
}