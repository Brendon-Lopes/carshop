using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Brand;
using Carshop.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carshop.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandsController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet(Name = "GetAllBrands")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var brands = await _brandService.GetAll();

        return Ok(brands);
    }

    [HttpGet("{id}", Name = "GetBrandById")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var brand = await _brandService.GetById(id);

        return Ok(brand);
    }

    [HttpPost(Name = "SaveBrand")]
    public async Task<IActionResult> Save(BrandDTO brandDto)
    {
        var brand = await _brandService.Save(brandDto);

        return CreatedAtAction(nameof(GetById), new {id = brand.Id}, brand);
    }
}