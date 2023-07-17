using Carshop.Application.DTOs;

namespace Carshop.Application.Interfaces.Car;

public interface ICarService
{
    Task<Domain.Models.Car> Save(CarDTO car);
}