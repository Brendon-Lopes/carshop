using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Brand;
using Carshop.Application.Interfaces.Car;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;

namespace Carshop.Application.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;

    public CarService(
        ICarRepository carRepository,
        IBrandService brandService,
        IMapper mapper)
    {
        _carRepository = carRepository;
        _brandService = brandService;
        _mapper = mapper;
    }

    public async Task<GetAllCarsFilteredAndPaginatedResponse> GetAllCarsFilteredAndPaginated(
        int page,
        int pageSize,
        string name,
        string brandName,
        bool decrescentOrder)
    {
        var cars = await _carRepository.GetFilteredCars(name, brandName);

        var totalCars = cars.Count;

        var orderedCars = decrescentOrder
            ? cars.OrderByDescending(c => c.Price)
            : cars.OrderBy(c => c.Price);

        var carsWithPagination = orderedCars
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var pageCount = (int)Math.Ceiling((double)totalCars / pageSize);

        return new GetAllCarsFilteredAndPaginatedResponse
        {
            Cars = _mapper.Map<IEnumerable<CarResponse>>(carsWithPagination),
            TotalPages = pageCount,
            CurrentPage = page
        };
    }

    public async Task<CarResponse> Save(CarDTO car)
    {
        await _brandService.CheckIfBrandExists(car.BrandId);

        var carEntity = _mapper.Map<Car>(car);

        var savedCar = await _carRepository.Save(carEntity);

        return _mapper.Map<CarResponse>(savedCar);
    }
}