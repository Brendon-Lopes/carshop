using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Exceptions;
using Carshop.Application.Interfaces.Brand;
using Carshop.Application.Services;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Carshop.Application.UnitTests.Services;

public class BrandServiceTests
{
    private readonly Mock<IBrandRepository> _brandRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IBrandService _brandService;

    public BrandServiceTests()
    {
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _mapperMock = new Mock<IMapper>();
        _brandService = new BrandService(_brandRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetById_WithExistingBrand_ShouldReturnBrandResponse()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var brand = new Brand { Name = "Brand 1" };
        var brandResponse = new BrandResponse { Id = brandId, Name = "Brand 1" };

        _brandRepositoryMock.Setup(repo => repo.GetById(brandId)).ReturnsAsync(brand);
        _mapperMock.Setup(mapper => mapper.Map<BrandResponse>(brand)).Returns(brandResponse);

        // Act
        var result = await _brandService.GetById(brandId);

        // Assert
        result.Should().BeEquivalentTo(brandResponse);
    }

    [Fact]
    public async Task GetById_WithNonExistingBrand_ShouldThrowCustomException()
    {
        // Arrange
        var brandId = Guid.NewGuid();

        _brandRepositoryMock.Setup(repo => repo.GetById(brandId)).ReturnsAsync((Brand)null);

        // Act
        Func<Task> act = async () => await _brandService.GetById(brandId);

        // Assert
        await act.Should().ThrowAsync<CustomException>();
    }

    [Fact]
    public async Task GetAll_ShouldReturnListOfBrandResponses()
    {
        // Arrange
        var brands = new List<Brand>
        {
            new Brand { Name = "Brand 1" },
            new Brand { Name = "Brand 2" }
        };
        var brandResponses = new List<BrandResponse>
        {
            new BrandResponse { Id = brands[0].Id, Name = "Brand 1" },
            new BrandResponse { Id = brands[1].Id, Name = "Brand 2" }
        };

        _brandRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(brands);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<BrandResponse>>(brands)).Returns(brandResponses);

        // Act
        var result = await _brandService.GetAll();

        // Assert
        result.Should().BeEquivalentTo(brandResponses);
    }

    [Fact]
    public async Task Save_WithUniqueBrandName_ShouldReturnBrandResponse()
    {
        // Arrange
        var brandDto = new BrandDTO { Name = "Brand 1" };
        var newBrand = new Brand { Name = "Brand 1" };
        var createdBrand = new Brand { Name = "Brand 1" };
        var brandResponse = new BrandResponse { Id = newBrand.Id, Name = "Brand 1" };

        _mapperMock.Setup(mapper => mapper.Map<Brand>(brandDto)).Returns(newBrand);
        _brandRepositoryMock.Setup(repo => repo.GetByName(brandDto.Name)).ReturnsAsync((Brand)null);
        _brandRepositoryMock.Setup(repo => repo.Save(newBrand)).ReturnsAsync(createdBrand);
        _mapperMock.Setup(mapper => mapper.Map<BrandResponse>(createdBrand)).Returns(brandResponse);

        // Act
        var result = await _brandService.Save(brandDto);

        // Assert
        result.Should().BeEquivalentTo(brandResponse);
    }

    [Fact]
    public async Task Save_WithDuplicateBrandName_ShouldThrowCustomException()
    {
        // Arrange
        var brandDto = new BrandDTO { Name = "Brand 1" };
        var existingBrand = new Brand { Name = "Brand 1" };

        _brandRepositoryMock.Setup(repo => repo.GetByName(brandDto.Name)).ReturnsAsync(existingBrand);

        // Act
        Func<Task> act = async () => await _brandService.Save(brandDto);

        // Assert
        await act.Should().ThrowAsync<CustomException>()
            .WithMessage($"Brand with name '{brandDto.Name}' already exists");

    }
    [Fact]
    public async Task CheckIfBrandExists_WithExistingBrand_ShouldNotThrowException()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var brand = new Brand { Name = "Brand 1" };

        _brandRepositoryMock.Setup(repo => repo.GetById(brandId)).ReturnsAsync(brand);

        // Act
        Func<Task> act = async () => await _brandService.CheckIfBrandExists(brandId);

        // Assert
        await act.Should().NotThrowAsync<CustomException>();
    }

    [Fact]
    public async Task CheckIfBrandExists_WithNonExistingBrand_ShouldThrowCustomException()
    {
        // Arrange
        var brandId = Guid.NewGuid();

        _brandRepositoryMock.Setup(repo => repo.GetById(brandId)).ReturnsAsync((Brand)null);

        // Act
        Func<Task> act = async () => await _brandService.CheckIfBrandExists(brandId);

        // Assert
        await act.Should().ThrowAsync<CustomException>()
            .WithMessage($"Brand with ID {brandId} does not exist");
    }
}
