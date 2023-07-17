using System.Net;
using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Exceptions;
using Carshop.Application.Interfaces.Authentication;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;

namespace Carshop.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHandler _passwordHandler;
    private readonly IMapper _mapper;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        IPasswordHandler passwordHandler,
        IMapper mapper)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _passwordHandler = passwordHandler;
        _mapper = mapper;
    }

    public async Task<AuthenticationResponse> Register(UserDTO userDto)
    {
        // check if user exists
        var findUser = await _userRepository.GetByEmail(userDto.Email);

        if (findUser is not null)
            throw new CustomException("Email already registered", HttpStatusCode.Conflict);

        // hash password
        var user = _mapper.Map<User>(userDto);
        user.Password = _passwordHandler.HashPassword(userDto.Password);

        // save user
        var savedUser = await _userRepository.Save(user);

        // create token
        var token = _jwtTokenGenerator.GenerateToken(savedUser.Id,
            savedUser.FirstName,
            savedUser.LastName,
            savedUser.Email,
            savedUser.Role);

        // map user to response
        var response = _mapper.Map<AuthenticationResponse>(user);
        response.Token = token;

        return response;
    }

    public async Task<AuthenticationResponse> Login(string email, string password)
    {
        throw new NotImplementedException();
    }
}