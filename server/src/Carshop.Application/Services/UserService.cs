using System.Net;
using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Exceptions;
using Carshop.Application.Interfaces.User;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;

namespace Carshop.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<User> GetById(Guid id)
    {
        var user = await _userRepository.GetById(id);

        if (user is null)
            throw new CustomException("User not found", HttpStatusCode.NotFound);

        return user;
    }

    public async Task<User> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmail(email);

        if (user is null)
            throw new CustomException("User not found", HttpStatusCode.NotFound);

        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User> Save(UserDTO user)
    {
        await CheckIfUserExists(user.Email);

        var newUser = _mapper.Map<User>(user);

        var created = await _userRepository.Save(newUser);

        return created;
    }

    public async Task<User> Update(UserDTO user)
    {
        var existingUser = await _userRepository.GetByEmail(user.Email);

        if (existingUser is null)
            throw new CustomException("User not found", HttpStatusCode.NotFound);

        _mapper.Map(user, existingUser);

        var updated = await _userRepository.Update(existingUser);

        return updated;
    }

    private async Task CheckIfUserExists(string email)
    {
        var existingUser = await _userRepository.GetByEmail(email);

        if (existingUser is not null)
            throw new CustomException($"Email already registered", HttpStatusCode.Conflict);
    }
}