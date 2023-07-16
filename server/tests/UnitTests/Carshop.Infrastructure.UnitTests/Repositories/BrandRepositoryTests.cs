using System;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;
using Carshop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Carshop.Infrastructure.UnitTests.Repositories;

public class BrandRepositoryTests
{
    private readonly DbContextOptions<AppDbContext> _options;

    public BrandRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async void GetAll_ReturnsAllBrands()
    {
        // Arrange
        var brands = new Faker<Brand>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Name, f => f.Company.CompanyName())
            .Generate(3);

        await using (var context = new AppDbContext(_options))
        {
            await context.Brands.AddRangeAsync(brands);
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(_options))
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
        Guid id;
        var brand = new Brand { Name = "Test Brand" };

        await using (var context = new AppDbContext(_options))
        {
            var created = await context.Brands.AddAsync(brand);

            id = created.Entity.Id;

            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(_options))
        {
            var repository = new BrandRepository(context);

            // Act
            var result = await repository.GetById(id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be("Test Brand");
        }
    }

    [Fact]
    public async void Save_Should_AddBrandToDatabaseAndReturnCreatedEntity()
    {
        // Arrange
        await using var context = new AppDbContext(_options);

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
    public async void GetByName_ShouldReturnBrand_WhenBrandExists()
    {
        // Arrange
        var brandName = new Faker().Company.CompanyName();
        var brand = new Brand { Name = brandName };

        await using (var context = new AppDbContext(_options))
        {
            var repository = new BrandRepository(context);
            await repository.Save(brand);
        }

        // Act
        await using (var context = new AppDbContext(_options))
        {
            var repository = new BrandRepository(context);
            var result = await repository.GetByName(brandName);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be(brandName);
        }
    }
}