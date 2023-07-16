using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Bogus;
using FluentAssertions;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Repositories;
using Carshop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carshop.Infrastructure.UnitTests.Repositories;

public class RepositoryTests
{
    private readonly Faker<Car> _carFaker;
    private readonly DbContextOptions<AppDbContext> _options;

    public RepositoryTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _carFaker = new Faker<Car>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Vehicle.Model());
    }

    [Fact]
    public async Task GetById_ShouldReturnEntity_WhenIdExists()
    {
        // Arrange
        var car = _carFaker.Generate();

        await using (var context = new AppDbContext(_options))
        {
            context.Set<Car>().Add(car);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(_options))
        {
            var repository = new Repository<Car>(context);

            var result = await repository.GetById(car.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(car);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        await using var context = new AppDbContext(_options);

        var repository = new Repository<Car>(context);
        var result = await repository.GetById(nonExistingId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var cars = _carFaker.Generate(3);

        await using (var context = new AppDbContext(_options))
        {
            await context.Set<Car>().AddRangeAsync(cars);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(_options))
        {
            var repository = new Repository<Car>(context);

            var result = await repository.GetAll();
            var resultList = result.ToList();

            // Assert
            resultList.Count.Should().Be(3);

            foreach (var car in cars)
            {
                resultList.Should().Contain(c => car.Name == c.Name);
            }
        }
    }

    [Fact]
    public async Task Save_ShouldCreateEntity()
    {
        // Arrange
        var car = _carFaker.Generate();

        // Act
        await using (var context = new AppDbContext(_options))
        {
            var repository = new Repository<Car>(context);

            var result = await repository.Save(car);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(car);
        }

        // Assert
        await using (var context = new AppDbContext(_options))
        {
            var result = await context.Set<Car>().FirstOrDefaultAsync(c => c.Id == car.Id);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(car);
        }
    }

    [Fact]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        var car = _carFaker.Generate();

        await using (var context = new AppDbContext(_options))
        {
            context.Set<Car>().Add(car);
            await context.SaveChangesAsync();
        }

        car.Name = "Updated Car";

        // Act
        await using (var context = new AppDbContext(_options))
        {
            var repository = new Repository<Car>(context);

            var result = await repository.Update(car);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(car);
        }

        // Assert
        await using (var context = new AppDbContext(_options))
        {
            var result = await context.Set<Car>().FirstOrDefaultAsync(c => c.Id == car.Id);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(car);
        }
    }

    [Fact]
    public async Task Delete_ShouldDeleteEntity()
    {
        // Arrange
        var car = _carFaker.Generate();

        await using (var context = new AppDbContext(_options))
        {
            context.Set<Car>().Add(car);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(_options))
        {
            var repository = new Repository<Car>(context);

            await repository.Delete(car);
        }

        // Assert
        await using (var context = new AppDbContext(_options))
        {
            var result = await context.Set<Car>().FirstOrDefaultAsync(c => c.Id == car.Id);

            result.Should().BeNull();
        }
    }
}