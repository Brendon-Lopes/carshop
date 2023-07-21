using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Carshop.API.Controllers;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Brand;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Carshop.API.UnitTests.Controllers;

public class BrandsControllerTests
{
    private readonly Mock<IBrandService> _mockBrandService;
    private readonly BrandsController _controller;

    public BrandsControllerTests()
    {
        _mockBrandService = new Mock<IBrandService>();
        _controller = new BrandsController(_mockBrandService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithListOfBrands()
    {
        // Arrange
        var brands = GenerateBrandResponse(3);

        _mockBrandService.Setup(service => service.GetAll())
            .ReturnsAsync(brands);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(brands);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WithBrandDto()
    {
        // Arrange
        var brand = GenerateBrandResponse(1)[0];

        _mockBrandService.Setup(service => service.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(brand);

        // Act
        var result = await _controller.GetById(brand.Id);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeEquivalentTo(brand);
    }

    [Fact]
    public async Task Save_ReturnsCreated_WithBrandDto()
    {
        // Arrange
        var brand = GenerateBrandResponse(1)[0];
        var brandDto = new BrandDTO { Name = brand.Name };

        _mockBrandService.Setup(service => service.Save(It.IsAny<BrandDTO>()))
            .ReturnsAsync(brand);

        // Act
        var result = await _controller.Save(brandDto);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result);

        actionResult.Value.Should().BeEquivalentTo(brand);
    }

    private static IList<BrandResponse> GenerateBrandResponse(int count)
    {
        return new Faker<BrandResponse>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Name, f => f.Vehicle.Manufacturer())
            .Generate(count);
    }
}