using Carshop.Application.Interfaces.Authentication;
using Carshop.Infrastructure.Authentication;
using FluentAssertions;
using Xunit;

namespace Carshop.Infrastructure.UnitTests.Authentication
{
    public class PasswordHandlerTests
    {
        private readonly IPasswordHandler _passwordHandler;

        public PasswordHandlerTests()
        {
            _passwordHandler = new PasswordHandler();
        }

        [Fact]
        public void HashPassword_ShouldReturnNonEmptyHash()
        {
            // Arrange
            var password = "password";

            // Act
            var hash = _passwordHandler.HashPassword(password);

            // Assert
            hash.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "password";
            var hash = _passwordHandler.HashPassword(password);

            // Act
            var result = _passwordHandler.VerifyPassword(password, hash);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var password = "password";
            var incorrectPassword = "incorrect";
            var hash = _passwordHandler.HashPassword(password);

            // Act
            var result = _passwordHandler.VerifyPassword(incorrectPassword, hash);

            // Assert
            result.Should().BeFalse();
        }
    }
}