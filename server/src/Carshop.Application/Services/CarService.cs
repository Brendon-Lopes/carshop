using System.Net;
using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Exceptions;
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

    public async Task<CarResponse> Update(Guid id, CarDTO carDto)
    {
        var carEntity = await _carRepository.GetById(id);

        if (carEntity is null)
            throw new CustomException($"Car with ID {id} does not exist", HttpStatusCode.BadRequest);

        await _brandService.CheckIfBrandExists(carDto.BrandId);

        _mapper.Map(carDto, carEntity);

        var updatedCar = await _carRepository.Update(carEntity);

        return _mapper.Map<CarResponse>(updatedCar);
    }

    public async Task Delete(Guid id)
    {
        var carEntity = await _carRepository.GetById(id);

        if (carEntity is null)
            throw new CustomException($"Car with ID {id} does not exist", HttpStatusCode.BadRequest);

        await _carRepository.Delete(carEntity);
    }
}