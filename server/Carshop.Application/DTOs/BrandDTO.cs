using System.ComponentModel.DataAnnotations;

namespace Carshop.Application.DTOs;

public class BrandDTO
{
    [Required]
    [MinLength(2, ErrorMessage = "Brand 'Name' must be at least 2 characters long")]
    public string Name { get; set; } = string.Empty;
}