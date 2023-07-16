using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Brand;
using Carshop.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carshop.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;

    public BrandsController(IBrandService brandService, IMapper mapper)
    {
        _brandService = brandService;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetBrands")]
    public async Task<IEnumerable<Brand>> Get()
    {
        return await _brandService.GetAll();
    }

    [HttpGet("{id}", Name = "GetBrandById")]
    public async Task<Brand> GetById(Guid id)
    {
        return await _brandService.GetById(id);
    }

    [HttpPost(Name = "SaveBrand")]
    public async Task<IActionResult> Save(BrandDTO brandDto)
    {
        var brand = await _brandService.Save(brandDto);

        var response = _mapper.Map<BrandResponse>(brand);

        return CreatedAtAction(nameof(GetById), new {id = brand.Id}, response);
    }
}