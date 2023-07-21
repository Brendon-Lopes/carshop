using System.Threading.Tasks;
using Carshop.API.Controllers;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Xunit;
using Moq;

namespace Carshop.API.UnitTests.Controllers;

public class AuthenticationControllerTests
{
    private readonly Mock<IAuthenticationService> _mockAuthService;
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _mockAuthService = new Mock<IAuthenticationService>();

        _controller = new AuthenticationController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Register_CallsRegisterOnAuthService()
    {
        // Arrange
        var userDto = new UserDTO();

        _mockAuthService.Setup(service => service.Register(It.IsAny<UserDTO>()))
            .ReturnsAsync(new AuthenticationResponse());

        // Act
        await _controller.Register(userDto);

        // Assert
        _mockAuthService.Verify(service => service.Register(It.IsAny<UserDTO>()), Times.Once);
    }

    [Fact]
    public async Task Register_ReturnsCreatedResult_OnSuccess()
    {
        // Arrange
        var userDto = new UserDTO();
        var authResponse = new AuthenticationResponse();

        _mockAuthService.Setup(service => service.Register(It.IsAny<UserDTO>()))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _controller.Register(userDto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }
}
