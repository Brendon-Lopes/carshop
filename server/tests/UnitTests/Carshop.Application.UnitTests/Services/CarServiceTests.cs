using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Brand;
using Carshop.Application.Services;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using Moq;
using Bogus;
using Xunit;

namespace Carshop.Application.UnitTests.Services;

public class CarServiceTests
{
    private Mock<ICarRepository> _carRepositoryMock;
    private Mock<IBrandService> _brandServiceMock;
    private Mock<IMapper> _mapperMock;
    private CarService _carService;

    public CarServiceTests()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _brandServiceMock = new Mock<IBrandService>();
        _mapperMock = new Mock<IMapper>();
        _carService = new CarService(_carRepositoryMock.Object, _brandServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllCarsFilteredAndPaginated_ShouldReturnCorrectResult()
    {
        // Arrange
        var cars = new List<Car> { GenerateFakeCar(), GenerateFakeCar() };
        _carRepositoryMock.Setup(repo => repo.GetFilteredCars(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(cars);

        // Act
        var result = await _carService.GetAllCarsFilteredAndPaginated(1, 2, null, null, false);

        // Assert
        Assert.Equal(1, result.TotalPages);
        _carRepositoryMock.Verify(x => x.GetFilteredCars(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Save_ShouldCallCheckIfBrandExistsAndSave()
    {
        // Arrange
        var carDto = GenerateFakeCarDto();
        var car = GenerateFakeCar();
        _mapperMock.Setup(m => m.Map<Car>(carDto)).Returns(car);
        _carRepositoryMock.Setup(repo => repo.Save(car)).ReturnsAsync(car);

        // Act
        await _carService.Save(carDto);

        // Assert
        _brandServiceMock.Verify(bs => bs.CheckIfBrandExists(carDto.BrandId), Times.Once);
        _carRepositoryMock.Verify(cr => cr.Save(car), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldCallCheckIfBrandExistsAndUpdate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var carDto = GenerateFakeCarDto();
        var car = GenerateFakeCar();
        _carRepositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(car);
        _carRepositoryMock.Setup(repo => repo.Update(car)).ReturnsAsync(car);

        // Act
        await _carService.Update(id, carDto);

        // Assert
        _brandServiceMock.Verify(bs => bs.CheckIfBrandExists(carDto.BrandId), Times.Once);
        _carRepositoryMock.Verify(cr => cr.Update(car), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldCallGetByIdAndDelete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var car = GenerateFakeCar();
        _carRepositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(car);

        // Act
        await _carService.Delete(id);

        // Assert
        _carRepositoryMock.Verify(cr => cr.GetById(id), Times.Once);
        _carRepositoryMock.Verify(cr => cr.Delete(car), Times.Once);
    }

    private static Car GenerateFakeCar()
    {
        return new Faker<Car>()
            .RuleFor(car => car.Name, faker => faker.Vehicle.Manufacturer())
            .RuleFor(car => car.Model, faker => faker.Vehicle.Model())
            .RuleFor(car => car.ImageUrl, faker => faker.Internet.Url())
            .RuleFor(car => car.Year, faker => faker.Random.Int(1900, 2100))
            .RuleFor(car => car.Price, faker => faker.Random.Decimal(0.01m, 1000000m))
            .RuleFor(car => car.BrandId, faker => Guid.NewGuid())
            .Generate();
    }

    private static CarDTO GenerateFakeCarDto()
    {
        return new Faker<CarDTO>()
            .RuleFor(carDto => carDto.Name, faker => faker.Vehicle.Manufacturer())
            .RuleFor(carDto => carDto.Model, faker => faker.Vehicle.Model())
            .RuleFor(carDto => carDto.ImageUrl, faker => faker.Internet.Url())
            .RuleFor(carDto => carDto.Year, faker => faker.Random.Int(1900, 2100))
            .RuleFor(carDto => carDto.Price, faker => faker.Random.Decimal(0.01m, 1000000m))
            .RuleFor(carDto => carDto.BrandId, faker => Guid.NewGuid())
            .Generate();
    }
}
