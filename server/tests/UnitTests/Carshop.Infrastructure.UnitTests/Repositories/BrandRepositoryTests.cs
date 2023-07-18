using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;
using Carshop.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Carshop.Infrastructure.UnitTests.Repositories;

public class BrandRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly BrandRepository _repository;

    public BrandRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new BrandRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAll_ReturnsAllBrands()
    {
        // Arrange
        var brands = new Faker<Brand>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Name, f => f.Company.CompanyName())
            .Generate(3);

        await _context.Brands.AddRangeAsync(brands);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAll();
        var resultList = result.ToList();

        // Assert
        resultList.Count.Should().Be(3);

        foreach (var brand in brands)
        {
            resultList.Should().Contain(b => brand.Name == b.Name);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnBrand_WhenValidIdIsProvided()
    {
        // Arrange
        var brand = new Brand { Name = "Test Brand" };

        var created = await _context.Brands.AddAsync(brand);
        await _context.SaveChangesAsync();
        var id = created.Entity.Id;

        // Act
        var result = await _repository.GetById(id);
        var count = await _context.Brands.CountAsync();

        // Assert
        count.Should().Be(1);
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Name.Should().Be("Test Brand");
    }

    [Fact]
    public async Task Save_Should_AddBrandToDatabaseAndReturnCreatedEntity()
    {
        // Arrange
        var brand = new Faker<Brand>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Name, f => f.Vehicle.Manufacturer())
            .Generate();

        // Act
        var createdBrand = await _repository.Save(brand);

        // Assert
        _context.Brands.Should().Contain(brand);
        createdBrand.Should().BeEquivalentTo(brand);
    }

    [Fact]
    public async Task GetByName_ShouldReturnBrand_WhenBrandExists()
    {
        // Arrange
        var brand = new Brand { Name = "brandName" };

        await _context.Set<Brand>().AddAsync(brand);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByName("brandName");

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("brandName");
    }

    [Fact]
    public async Task Update_ShouldUpdateBrand_WhenBrandExists()
    {
        // Arrange
        var brandName = new Faker().Vehicle.Manufacturer();
        var brand = new Brand { Name = brandName };

        await _context.Set<Brand>().AddAsync(brand);
        await _context.SaveChangesAsync();

        // Act
        var newBrandName = new Faker().Vehicle.Manufacturer();
        brand.Name = newBrandName;

        await _repository.Update(brand);
        var result = await _repository.GetById(brand.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(newBrandName);
    }
}