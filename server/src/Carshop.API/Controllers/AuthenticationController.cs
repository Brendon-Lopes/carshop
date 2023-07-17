using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Carshop.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register", Name = "Register")]
    public async Task<IActionResult> Register(UserDTO userDto)
    {
        var response = await _authenticationService.Register(userDto);

        return CreatedAtAction(nameof(Login), new {email = userDto.Email}, response);
    }

    [HttpPost("login", Name = "Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var response = await _authenticationService.Login(email, password);

        return Ok(response);
    }
}