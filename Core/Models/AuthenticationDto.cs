using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class AuthenticationDto
{
    [Required] public string? Email { get; set; }

    [Required] public string? Password { get; set; }
}