namespace Carshop.Application.DTOs;

public class CarResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public int Year { get; init; }
    public decimal Price { get; init; }
    public string BrandName { get; init; } = string.Empty;
    public Guid BrandId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}