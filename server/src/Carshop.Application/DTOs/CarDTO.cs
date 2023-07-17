using System.ComponentModel.DataAnnotations;

namespace Carshop.Application.DTOs;

public class CarDTO
{
    [Required, MinLength(2, ErrorMessage = "Car 'Name' must be at least 2 characters long")]
    public string Name { get; set; } = string.Empty;

    [Required, MinLength(2, ErrorMessage = "Car 'Model' must be at least 2 characters long")]
    public string Model { get; set; } = string.Empty;

    [Required, Url(ErrorMessage = "Car 'ImageUrl' must be a valid URL")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required, Range(1900, 2100, ErrorMessage = "Car 'Year' must be between 1900 and 2100")]
    public int Year { get; set; }

    [Required, Range(0.01, double.MaxValue, ErrorMessage = "Car 'Price' must be bigger than 0")]
    public decimal Price { get; set; }

    [Required]
    public Guid BrandId { get; set; }
}