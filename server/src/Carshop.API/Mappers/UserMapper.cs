using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Domain.Models;

namespace Carshop.API.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
         CreateMap<UserDTO, User>().ReverseMap();
         CreateMap<AuthenticationResponse, User>().ReverseMap();
    }
}