using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Car;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;

namespace Carshop.Application.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public CarService(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<Car> Save(CarDTO car)
    {
        var carEntity = _mapper.Map<Car>(car);

        return await _carRepository.Save(carEntity);
    }
}