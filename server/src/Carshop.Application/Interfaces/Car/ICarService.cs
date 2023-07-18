using Carshop.Application.DTOs;

namespace Carshop.Application.Interfaces.Car;

public interface ICarService
{
    Task<GetAllCarsFilteredAndPaginatedResponse> GetAllCarsFilteredAndPaginated(
        int page,
        int pageSize,
        string name,
        string brandName,
        bool decrescentOrder = true);

    Task<CarResponse> Save(CarDTO car);

    Task<CarResponse> Update(Guid id, CarDTO car);
}