using System.ComponentModel.DataAnnotations;

namespace Carshop.Application.DTOs;

public class UserDTO
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(2, ErrorMessage = "User 'FirstName' must be at least 2 characters long")]
    public string FirstName { get; set; } = string.Empty;

    [Required, MinLength(2, ErrorMessage = "User 'LastName' must be at least 2 characters long")]
    public string LastName { get; set; } = string.Empty;

    [Required, MinLength(6, ErrorMessage = "User 'Password' must be at least 6 characters long")]
    public string Password { get; set; } = string.Empty;
}