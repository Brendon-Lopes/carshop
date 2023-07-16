using System.ComponentModel.DataAnnotations;

namespace Carshop.Domain.Models;

public class BaseEntity
{
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; set; }
}