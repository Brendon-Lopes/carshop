using System;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;
using Carshop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Carshop.Infrastructure.UnitTests.Repositories;

public class BrandRepositoryTests
{
    [Fact]
    public async Task GetAll_ReturnsAllBrands()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var brands = new Faker<Brand>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Name, f => f.Company.CompanyName())
            .Generate(3);

        await using (var context = new AppDbContext(options))
        {
            await context.Brands.AddRangeAsync(brands);
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var repository = new BrandRepository(context);

            // Act
            var result = await repository.GetAll();
            var resultList = result.ToList();

            // Assert
            resultList.Count.Should().Be(3);

            foreach (var brand in brands)
            {
                resultList.Should().Contain(b => brand.Name == b.Name);
            }
        }
    }

    [Fact]
    public async void GetById_ShouldReturnBrand_WhenValidIdIsProvided()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Guid id;
        var brand = new Brand { Name = "Test Brand" };

        await using (var context = new AppDbContext(options))
        {
            var created = await context.Brands.AddAsync(brand);

            await context.SaveChangesAsync();

            id = created.Entity.Id;
        }

        await using (var context = new AppDbContext(options))
        {
            var repository = new BrandRepository(context);

            // Act
            var result = await repository.GetById(id);
            var count = await context.Brands.CountAsync();

            // Assert
            count.Should().Be(1);
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be("Test Brand");
        }
    }

    [Fact]
    public async Task Save_Should_AddBrandToDatabaseAndReturnCreatedEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        // Create the repository instance
        var repository = new BrandRepository(context);

        // Create a brand entity using Bogus library
        var brand = new Faker<Brand>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Name, f => f.Vehicle.Manufacturer())
            .Generate();

        // Act
        var createdBrand = await repository.Save(brand);

        // Assert
        context.Brands.Should().Contain(brand);

        createdBrand.Should().BeEquivalentTo(brand);
    }

    [Fact]
    public async Task GetByName_ShouldReturnBrand_WhenBrandExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var brand = new Brand { Name = "brandName" };

        await using (var context = new AppDbContext(options))
        {
            await context.Set<Brand>().AddAsync(brand);
            await context.SaveChangesAsync();
        }

        // Act
        await using (var context = new AppDbContext(options))
        {
            var repository = new BrandRepository(context);
            var result = await repository.GetByName("brandName");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("brandName");
        }
    }
}