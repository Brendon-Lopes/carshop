using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Domain.Models;

namespace Carshop.API.Mappers;

public class BrandMapper : Profile
{
    public BrandMapper()
    {
        CreateMap<BrandDTO, Brand>().ReverseMap();
        CreateMap<Brand, BrandResponse>().ReverseMap();
    }
}