using Carshop.Domain.Models;

namespace Carshop.Domain.Interfaces;

public interface IBrandRepository : IRepository<Brand>
{
    Task<Brand?> GetByName(string name);
}