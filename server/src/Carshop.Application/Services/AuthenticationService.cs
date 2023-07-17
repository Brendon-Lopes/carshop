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
        var findUser = await _userRepository.GetByEmail(userDto.Email);

        if (findUser is not null)
            throw new CustomException("Email already registered", HttpStatusCode.Conflict);

        var user = _mapper.Map<User>(userDto);
        user.Password = _passwordHandler.HashPassword(userDto.Password);

        var savedUser = await _userRepository.Save(user);

        var token = _jwtTokenGenerator.GenerateToken(savedUser);

        var response = _mapper.Map<AuthenticationResponse>(user);
        response.Token = token;

        return response;
    }

    public async Task<AuthenticationResponse> Login(UserLoginDTO userLoginDto)
    {
        var user = await _userRepository.GetByEmail(userLoginDto.Email);

        if (user is null)
            throw new CustomException("Invalid email or password", HttpStatusCode.Unauthorized);

        if (!_passwordHandler.VerifyPassword(userLoginDto.Password, user.Password))
            throw new CustomException("Invalid email or password", HttpStatusCode.Unauthorized);

        var token = _jwtTokenGenerator.GenerateToken(user);

        var response = _mapper.Map<AuthenticationResponse>(user);
        response.Token = token;

        return response;
    }
}