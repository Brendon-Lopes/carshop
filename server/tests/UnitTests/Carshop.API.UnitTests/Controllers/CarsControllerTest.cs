using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using Carshop.API.Controllers;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Car;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Carshop.API.UnitTests.Controllers;

public class CarsControllerTest
{
    private readonly Mock<ICarService> _mockCarService;
    private readonly IMapper _mapper;

    public CarsControllerTest()
    {
        _mockCarService = new Mock<ICarService>();
        _mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<CarDTO, CarResponse>().ReverseMap();
        }).CreateMapper();
    }

    [Fact]
    public async Task GetAllCarsFilteredAndPaginated_ReturnsOk_WithListOfCars()
    {
        // Arrange
        var carsResponse = GenerateCarResponse(3);

        var carsFilteredAndPaginated = new GetAllCarsFilteredAndPaginatedResponse
        {
            Cars = carsResponse,
            CurrentPage = 1,
            TotalPages = 1
        };

        _mockCarService.Setup(service => service.GetAllCarsFilteredAndPaginated(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(carsFilteredAndPaginated);

        var controller = new CarsController(_mockCarService.Object);

        // Act
        var result = await controller.GetAllCarsFilteredAndPaginated();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(carsFilteredAndPaginated);
    }

    [Fact]
    public async Task Save_ReturnsCreated_WithCarDto()
    {
        // Arrange
        var carDto = GenerateCarDto(1)[0];

        var carResponse = _mapper.Map<CarResponse>(carDto);

        _mockCarService.Setup(service => service.Save(carDto))
            .ReturnsAsync(carResponse);

        var controller = new CarsController(_mockCarService.Object);

        // Act
        var result = await controller.Save(carDto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.Value.Should().BeEquivalentTo(carResponse);
    }

    [Fact]
    public async Task Update_ReturnsOk_WithCarDto()
    {
        // Arrange
        var carDto = GenerateCarDto(1)[0];

        var carResponse = _mapper.Map<CarResponse>(carDto);

        _mockCarService.Setup(service => service.Update(It.IsAny<Guid>(), carDto))
            .ReturnsAsync(carResponse);

        var controller = new CarsController(_mockCarService.Object);

        // Act
        var result = await controller.Update(It.IsAny<Guid>(), carDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(carResponse);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange
        _mockCarService.Setup(service => service.Delete(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        var controller = new CarsController(_mockCarService.Object);

        // Act
        var result = await controller.Delete(It.IsAny<Guid>());

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    private static IEnumerable<CarResponse> GenerateCarResponse(int quantity)
    {
        var faker = new Faker<CarResponse>()
            .RuleFor(car => car.Id, faker => faker.Random.Guid())
            .RuleFor(car => car.Name, faker => faker.Vehicle.Model())
            .RuleFor(car => car.BrandName, faker => faker.Vehicle.Manufacturer())
            .RuleFor(car => car.Price, faker => faker.Random.Decimal(10000, 100000))
            .RuleFor(car => car.Year, faker => faker.Random.Int(2000, 2021))
            .RuleFor(car => car.ImageUrl, faker => faker.Image.PicsumUrl());

        return faker.Generate(quantity);
    }

    private static List<CarDTO> GenerateCarDto(int quantity)
    {
        var faker = new Faker<CarDTO>()
            .RuleFor(car => car.Name, faker => faker.Vehicle.Model())
            .RuleFor(car => car.Model, faker => faker.Vehicle.Model())
            .RuleFor(car => car.ImageUrl, faker => faker.Image.PicsumUrl())
            .RuleFor(car => car.Year, faker => faker.Random.Int(2000, 2021))
            .RuleFor(car => car.Price, faker => faker.Random.Decimal(10000, 100000))
            .RuleFor(car => car.BrandId, faker => faker.Random.Guid());

        return faker.Generate(quantity);
    }
}