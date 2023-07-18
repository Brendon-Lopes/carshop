using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;
using Carshop.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Xunit;

namespace Carshop.Infrastructure.UnitTests.Repositories;

public class CarRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ICarRepository _repository;

    public CarRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new CarRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetFilteredCars_ShouldReturnFilteredCars_WhenNameAndBrandNameProvided()
    {
        // Arrange
        var brand = new Brand { Name = "Test Brand" };

        var cars = new Faker<Car>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Vehicle.Model())
            .RuleFor(c => c.Model, f => f.Vehicle.Type())
            .RuleFor(c => c.Brand, brand)
            .Generate(3);

        await _context.Brands.AddAsync(brand);
        await _context.Cars.AddRangeAsync(cars);
        await _context.SaveChangesAsync();

        var name = cars[0].Name;
        var brandName = cars[0].Brand.Name;

        // Act
        var result = await _repository.GetFilteredCars(name, brandName);

        // Assert
        result.Should().HaveCount(1);
        result[0].Name.Should().Be(name);
        result[0].Brand.Name.Should().Be(brandName);
    }

    [Fact]
    public async Task GetFilteredCars_ShouldReturnFilteredCars_WhenOnlyNameProvided()
    {
        // Arrange
        var brand = new Brand { Name = "Test Brand" };
        var cars = new List<Car>
        {
            new Car { Name = "Car 1", Model = "Model 1", Brand = brand },
            new Car { Name = "Car 2", Model = "Model 2", Brand = brand },
            new Car { Name = "Car 3", Model = "Model 3", Brand = brand }
        };

        await _context.Brands.AddAsync(brand);
        await _context.Cars.AddRangeAsync(cars);
        await _context.SaveChangesAsync();

        var name = "Car 1";

        // Act
        var result = await _repository.GetFilteredCars(name, string.Empty);

        // Assert
        result.Should().HaveCount(1);
        result[0].Name.Should().Be(name);
    }

    [Fact]
    public async Task GetFilteredCars_ShouldReturnFilteredCars_WhenOnlyBrandNameProvided()
    {
        // Arrange
        var brand = new Brand { Name = "Test Brand" };
        var cars = new List<Car>
        {
            new Car { Name = "Car 1", Model = "Model 1", Brand = brand },
            new Car { Name = "Car 2", Model = "Model 2", Brand = brand },
            new Car { Name = "Car 3", Model = "Model 3", Brand = brand }
        };

        await _context.Brands.AddAsync(brand);
        await _context.Cars.AddRangeAsync(cars);
        await _context.SaveChangesAsync();

        var brandName = "Test Brand";

        // Act
        var result = await _repository.GetFilteredCars(string.Empty, brandName);

        // Assert
        result.Should().HaveCount(3);
        result.Should().OnlyContain(c => c.Brand.Name == brandName);
    }

    [Fact]
    public async Task Save_Should_AddCarToDatabaseAndReturnCreatedEntity()
    {
        // Arrange
        var brand = new Brand { Name = "Test Brand" };
        await _context.Brands.AddAsync(brand);
        await _context.SaveChangesAsync();

        var car = new Car { Name = "Car 1", Model = "Model 1", BrandId = brand.Id };

        // Act
        var createdCar = await _repository.Save(car);

        // Assert
        createdCar.Should().BeEquivalentTo(car, options => options.Excluding(c => c.Brand));
        _context.Cars.Should().ContainEquivalentOf(car, options => options.Excluding(c => c.Brand));
    }
}