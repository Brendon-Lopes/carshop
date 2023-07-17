using Carshop.Application.DTOs;

namespace Carshop.Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> Register(UserDTO userDto);
    Task<AuthenticationResponse> Login(string email, string password);
}