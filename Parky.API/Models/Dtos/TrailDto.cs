using System.ComponentModel.DataAnnotations;
using static Parky.API.Models.Trail;

namespace Parky.API.Models.Dtos;

public class TrailDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public double Distance { get; set; }
    public DifficultyType Difficulty { get; set; }
    [Required]
    public int NationalParkId { get; set; }
    public NationalParkDto NationalPark { get; set; } = null!;
    [Required]
    public double Elevation { get; set; }
}
