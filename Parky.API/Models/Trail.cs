using System.ComponentModel.DataAnnotations;

namespace Parky.API.Models;

public class Trail
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public double Distance { get; set; }
    public enum DifficultyType { Easy, Moderate, Difficult, Expert }
    [Required]
    public double Elevation { get; set; }
    public DifficultyType Difficulty { get; set; }
    [Required]
    public int NationalParkId { get; set; }
    public NationalPark NationalPark { get; set; } = null!;
    public DateTime DateCreated { get; set; }
}
