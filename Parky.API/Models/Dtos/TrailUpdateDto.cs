using System.ComponentModel.DataAnnotations;
using static Parky.API.Models.Dtos.TrailDto;

namespace Parky.API.Models.Dtos;

public class TrailUpdateDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public double Distance { get; set; }
    public DifficultyType Difficulty { get; set; }
    [Required]
    public int NationalParkId { get; set; }
    [Required]
    public double Elevation { get; set; }
}
