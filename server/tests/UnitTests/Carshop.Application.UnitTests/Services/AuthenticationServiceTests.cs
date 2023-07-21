using System;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using Carshop.Application.DTOs;
using Carshop.Application.Exceptions;
using Carshop.Application.Interfaces.Authentication;
using Carshop.Application.Services;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Carshop.Application.UnitTests.Services;

public class AuthenticationServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGen;
    private readonly Mock<IPasswordHandler> _mockPasswordHandler;
    private readonly Mock<IMapper> _mockMapper;
    private readonly AuthenticationService _service;
    private readonly Faker<UserDTO> _userDtoFaker;
    private readonly Faker<UserLoginDTO> _userLoginDtoFaker;

    public AuthenticationServiceTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockJwtTokenGen = new Mock<IJwtTokenGenerator>();
        _mockPasswordHandler = new Mock<IPasswordHandler>();
        _mockMapper = new Mock<IMapper>();
        _service = new AuthenticationService(_mockJwtTokenGen.Object, _mockUserRepo.Object, _mockPasswordHandler.Object, _mockMapper.Object);
        _userDtoFaker = new Faker<UserDTO>();
        _userLoginDtoFaker = new Faker<UserLoginDTO>();
    }

    [Fact]
    public async Task Register_ShouldReturnAuthenticatedUser_WhenUserIsSuccessfullySaved()
    {
        // Arrange
        var fakeUserDto = _userDtoFaker.Generate();
        var fakeUser = new User();
        var fakeAuthResponse = new AuthenticationResponse();
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUserDto.Email)).ReturnsAsync((User)null);
        _mockMapper.Setup(m => m.Map<User>(It.Is<UserDTO>(u => u.Email == fakeUserDto.Email))).Returns(fakeUser);
        _mockPasswordHandler.Setup(p => p.HashPassword(fakeUserDto.Password)).Returns("hashedPassword");
        var savedUser = new User { Password = "hashedPassword" };
        _mockUserRepo.Setup(x => x.Save(It.Is<User>(u => u.Password == savedUser.Password))).ReturnsAsync(savedUser);
        _mockJwtTokenGen.Setup(j => j.GenerateToken(It.Is<User>(u => u.Password == savedUser.Password))).Returns("userToken");
        _mockMapper.Setup(m => m.Map<AuthenticationResponse>(It.Is<User>(u => u.Password == savedUser.Password))).Returns(fakeAuthResponse);

        // Act
        var result = await _service.Register(fakeUserDto);

        // Assert
        result.Should().BeEquivalentTo(fakeAuthResponse, options => options.ExcludingMissingMembers());
        _mockUserRepo.Verify(x => x.GetByEmail(It.Is<String>(s => s == fakeUserDto.Email)), Times.Once);
        _mockPasswordHandler.Verify(p => p.HashPassword(It.Is<String>(s => s == fakeUserDto.Password)), Times.Once);
        _mockUserRepo.Verify(x => x.Save(It.IsAny<User>()), Times.Once);
        _mockJwtTokenGen.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Once);
        _mockMapper.Verify(m => m.Map<AuthenticationResponse>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Register_ShouldThrowCustomException_WhenEmailIsAlreadyRegistered()
    {
        // Arrange
        var fakeUserDto = _userDtoFaker.Generate();
        var existingUser = new User();
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUserDto.Email)).ReturnsAsync(existingUser);

        // Act
        Func<Task> act = async () => await _service.Register(fakeUserDto);

        // Assert
        await act.Should().ThrowAsync<CustomException>().WithMessage("Email already registered");
        _mockUserRepo.Verify(x => x.GetByEmail(It.Is<String>(s => s == fakeUserDto.Email)), Times.Once);
        _mockUserRepo.Verify(x => x.Save(It.IsAny<User>()), Times.Never);
        _mockJwtTokenGen.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        _mockMapper.Verify(m => m.Map<AuthenticationResponse>(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Login_ShouldReturnAuthenticatedUser_WhenEmailAndPasswordAreCorrect()
    {
        // Arrange
        var fakeUserLoginDto = _userLoginDtoFaker.Generate();
        var existingUser = new User { Password = "hashedPassword" };
        var fakeAuthResponse = new AuthenticationResponse();
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUserLoginDto.Email)).ReturnsAsync(existingUser);
        _mockPasswordHandler.Setup(p => p.VerifyPassword(fakeUserLoginDto.Password, existingUser.Password)).Returns(true);
        _mockJwtTokenGen.Setup(j => j.GenerateToken(It.Is<User>(u => u.Password == existingUser.Password))).Returns("userToken");
        _mockMapper.Setup(m => m.Map<AuthenticationResponse>(It.Is<User>(u => u.Password == existingUser.Password))).Returns(fakeAuthResponse);

        // Act
        var result = await _service.Login(fakeUserLoginDto);

        // Assert
        result.Should().BeEquivalentTo(fakeAuthResponse, options => options.ExcludingMissingMembers());
        _mockUserRepo.Verify(x => x.GetByEmail(It.Is<String>(s => s == fakeUserLoginDto.Email)), Times.Once);
        _mockPasswordHandler.Verify(p => p.VerifyPassword(It.Is<String>(s => s == fakeUserLoginDto.Password), It.Is<String>(s => s == existingUser.Password)), Times.Once);
        _mockJwtTokenGen.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Once);
        _mockMapper.Verify(m => m.Map<AuthenticationResponse>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Login_ShouldThrowCustomException_WhenEmailIsNotFound()
    {
        // Arrange
        var fakeUserLoginDto = _userLoginDtoFaker.Generate();
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUserLoginDto.Email)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _service.Login(fakeUserLoginDto);

        // Assert
        await act.Should().ThrowAsync<CustomException>().WithMessage("Invalid email or password");
        _mockUserRepo.Verify(x => x.GetByEmail(It.Is<String>(s => s == fakeUserLoginDto.Email)), Times.Once);
        _mockPasswordHandler.Verify(p => p.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _mockJwtTokenGen.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        _mockMapper.Verify(m => m.Map<AuthenticationResponse>(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Login_ShouldThrowCustomException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var fakeUserLoginDto = _userLoginDtoFaker.Generate();
        var existingUser = new User { Password = "hashedPassword" };
        _mockUserRepo.Setup(x => x.GetByEmail(fakeUserLoginDto.Email)).ReturnsAsync(existingUser);
        _mockPasswordHandler.Setup(p => p.VerifyPassword(fakeUserLoginDto.Password, existingUser.Password))
            .Returns(false);

        // Act
        Func<Task> act = async () => await _service.Login(fakeUserLoginDto);

        // Assert
        await act.Should().ThrowAsync<CustomException>().WithMessage("Invalid email or password");
        _mockUserRepo.Verify(x => x.GetByEmail(It.Is<String>(s => s == fakeUserLoginDto.Email)), Times.Once);
        _mockPasswordHandler.Verify(
            p => p.VerifyPassword(It.Is<String>(s => s == fakeUserLoginDto.Password),
                It.Is<String>(s => s == existingUser.Password)), Times.Once);
        _mockJwtTokenGen.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        _mockMapper.Verify(m => m.Map<AuthenticationResponse>(It.IsAny<User>()), Times.Never);
    }
}
