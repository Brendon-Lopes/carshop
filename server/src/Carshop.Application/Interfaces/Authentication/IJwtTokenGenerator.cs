using Carshop.Domain.Enums;

namespace Carshop.Application.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string firstName, string lastName, string email, string role);
}