using System.ComponentModel.DataAnnotations;

namespace Parky.API.Models;

public class AuthenticationModel
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
