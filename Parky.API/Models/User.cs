using System.ComponentModel.DataAnnotations.Schema;

namespace Parky.API.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    [NotMapped]
    public string Token { get; set; } = string.Empty;
}
