using System;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using Carshop.Application.DTOs;
using Carshop.Application.Exceptions;
using Carshop.Application.Services;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Carshop.Application.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UserService _service;
    private readonly Faker<User> _userFaker;
    private readonly Faker<UserDTO> _userDtoFaker;

    public UserServiceTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new UserService(_mockUserRepo.Object, _mockMapper.Object);
        _userFaker = new Faker<User>();
        _userDtoFaker = new Faker<UserDTO>();
    }

    [Fact]
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var fakeUser = _userFaker.Generate();
        _mockUserRepo.Setup(x => x.GetById(fakeUser.Id)).ReturnsAsync(fakeUser);

        // Act
        var result = await _service.GetById(fakeUser.Id);

        // Assert
        result.Should().BeEquivalentTo(fakeUser, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task GetById_ShouldThrowCustomException_WhenUserNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockUserRepo.Setup(x => x.GetById(id)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _service.GetById(id);

        // Assert
        await act.Should().ThrowAsync<CustomException>().WithMessage("User not found");
    }

    [Fact]
    public async Task Save_ShouldReturnCreatedUser()
    {
        // Arrange
        var fakeUserDto = _userDtoFaker.Generate();
        var fakeUser = _userFaker.Generate();
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUserDto.Email)).ReturnsAsync((User)null);
        _mockMapper.Setup(m => m.Map<User>(It.IsAny<UserDTO>())).Returns(fakeUser);
        _mockUserRepo.Setup(x => x.Save(It.IsAny<User>())).ReturnsAsync(fakeUser);

        // Act
        var result = await _service.Save(fakeUserDto);

        // Assert
        result.Should().BeEquivalentTo(fakeUser, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Save_ShouldThrowCustomException_WhenEmailIsAlreadyRegistered()
    {
        // Arrange
        var fakeUserDto = _userDtoFaker.Generate();
        var fakeUser = _userFaker.Generate();
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUserDto.Email)).ReturnsAsync(fakeUser);

        // Act
        Func<Task> act = async () => await _service.Save(fakeUserDto);

        // Assert
        await act.Should().ThrowAsync<CustomException>().WithMessage("Email already registered");
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var fakeUser = _userFaker.Generate();
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUser.Email)).ReturnsAsync(fakeUser);

        // Act
        var result = await _service.GetByEmail(fakeUser.Email);

        // Assert
        result.Should().BeEquivalentTo(fakeUser, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task GetByEmail_ShouldThrowCustomException_WhenUserNotFound()
    {
        // Arrange
        var email = "nonexistentuser@test.com";
        _mockUserRepo.Setup(x => x.GetByEmail(email)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _service.GetByEmail(email);

        // Assert
        await act.Should().ThrowAsync<CustomException>().WithMessage("User not found");
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllUsers()
    {
        // Arrange
        var fakeUsers = _userFaker.Generate(5);
        _mockUserRepo.Setup(x => x.GetAll()).ReturnsAsync(fakeUsers);

        // Act
        var result = await _service.GetAll();

        // Assert
        result.Should().BeEquivalentTo(fakeUsers, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedUser_WhenUserExists()
    {
        // Arrange
        var fakeUserDto = _userDtoFaker.Generate();
        var existingUser = _userFaker.Generate();
        var updatedUser = _userFaker.Generate();
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUserDto.Email)).ReturnsAsync(existingUser);
        _mockUserRepo.Setup(x => x.Update(It.IsAny<User>())).ReturnsAsync(updatedUser);
        _mockMapper.Setup(m => m.Map<UserDTO, User>(fakeUserDto, existingUser)).Returns(updatedUser);

        // Act
        var result = await _service.Update(fakeUserDto);

        // Assert
        result.Should().BeEquivalentTo(updatedUser, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Update_ShouldThrowCustomException_WhenUserNotFound()
    {
        // Arrange
        var fakeUserDto = _userDtoFaker.Generate();
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUserDto.Email)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _service.Update(fakeUserDto);

        // Assert
        await act.Should().ThrowAsync<CustomException>().WithMessage("User not found");
    }

}
