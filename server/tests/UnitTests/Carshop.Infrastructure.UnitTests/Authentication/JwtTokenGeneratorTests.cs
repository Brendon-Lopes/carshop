using System.IdentityModel.Tokens.Jwt;
using AutoFixture;
using Bogus;
using Carshop.Domain.Enums;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Authentication;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Carshop.Infrastructure.UnitTests.Authentication;

public class JwtTokenGeneratorTests
{
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGeneratorTests()
    {
        var fixture = new Fixture();
        _jwtSettings = fixture.Create<JwtSettings>();

        var jwtOptions = Options.Create(_jwtSettings);

        _jwtTokenGenerator = new JwtTokenGenerator(jwtOptions);
    }

    [Fact]
    public void GenerateToken_ShouldGenerateTokenWithClaimsAndSettings()
    {
        // Arrange
        var user = new Faker<User>()
            .RuleFor(u => u.Role, UserRoles.Customer.ToString())
            .Generate();

        // Act
        var token = _jwtTokenGenerator.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();

        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(token);

        decodedToken.Issuer.Should().Be(_jwtSettings.Issuer);

        decodedToken.Audiences.Should().ContainSingle().And.Subject.Should().Equal(_jwtSettings.Audience);

        decodedToken.Claims.Should().Contain(c =>
            c.Type == JwtRegisteredClaimNames.NameId && c.Value == user.Id.ToString());
        decodedToken.Claims.Should().Contain(c =>
            c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
        decodedToken.Claims.Should().Contain(c =>
            c.Type == JwtRegisteredClaimNames.GivenName && c.Value == user.FirstName);
        decodedToken.Claims.Should().Contain(c =>
            c.Type == JwtRegisteredClaimNames.FamilyName && c.Value == user.LastName);
        decodedToken.Claims.Should().Contain(c =>
            c.Type == "role" && c.Value == user.Role);
    }


}