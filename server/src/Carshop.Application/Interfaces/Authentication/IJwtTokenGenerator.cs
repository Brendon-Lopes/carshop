namespace Carshop.Application.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(Domain.Models.User user);
}