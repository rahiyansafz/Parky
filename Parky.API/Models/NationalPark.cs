using System.ComponentModel.DataAnnotations;

namespace Parky.API.Models;

public class NationalPark
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string State { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public byte[] Picture { get; set; }
    public DateTime Established { get; set; }
}
