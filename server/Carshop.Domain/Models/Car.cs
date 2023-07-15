using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carshop.Domain.Models;

public class Car : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Year { get; set; }
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    [ForeignKey("BrandId")]
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
}