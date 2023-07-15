using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Brand;
using Carshop.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carshop.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;

    public BrandController(IBrandService brandService, IMapper mapper)
    {
        _brandService = brandService;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetBrands")]
    public IEnumerable<Brand> Get()
    {
        return _brandService.GetAll();
    }

    [HttpGet("{id}", Name = "GetBrandById")]
    public Brand GetById(Guid id)
    {
        return _brandService.GetById(id);
    }

    [HttpPost(Name = "SaveBrand")]
    public IActionResult Save(BrandDTO brandDto)
    {
        _brandService.Save(brandDto);

        return Ok();
    }
}