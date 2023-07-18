using Carshop.Domain.Models;

namespace Carshop.Domain.Interfaces;

public interface ICarRepository : IRepository<Car>
{
    Task<IList<Car>> GetFilteredCars(string name, string brandName);
}