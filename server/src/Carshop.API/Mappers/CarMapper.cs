using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Domain.Models;

namespace Carshop.API.Mappers;

public class CarMapper : Profile
{
    public CarMapper()
    {
        CreateMap<CarDTO, Car>().ReverseMap();
    }
}