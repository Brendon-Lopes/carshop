using System.ComponentModel.DataAnnotations.Schema;

namespace Carshop.Domain.Models;

public class Brand : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    [InverseProperty("Brand")]
    public ICollection<Car>? Cars { get; set; }
}