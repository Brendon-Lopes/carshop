using System.Net;
using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Exceptions;
using Carshop.Application.Interfaces.Brand;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;

namespace Carshop.Application.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public BrandService(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    public async Task<Brand> GetById(Guid id)
    {
        var brand = await _brandRepository.GetById(id);

        if (brand is null)
            throw new CustomException("Brand not found", HttpStatusCode.NotFound);

        return brand;
    }

    public async Task<IEnumerable<BrandResponse>> GetAll()
    {
        var brands = await _brandRepository.GetAll();

        var brandsResponse = _mapper.Map<IEnumerable<BrandResponse>>(brands);

        return brandsResponse;
    }

    public async Task<BrandResponse> Save(BrandDTO brand)
    {
        var newBrand = _mapper.Map<Brand>(brand);

        var findByName = await _brandRepository.GetByName(brand.Name);

        if (findByName is not null)
            throw new CustomException($"Brand with name '{brand.Name}' already exists", HttpStatusCode.Conflict);

        var created = await _brandRepository.Save(newBrand);

        var brandResponse = _mapper.Map<BrandResponse>(created);

        return brandResponse;
    }

    public async Task CheckIfBrandExists(Guid brandId)
    {
        var brand = await _brandRepository.GetById(brandId);

        if (brand is null)
            throw new CustomException($"Brand with ID {brandId} does not exist", HttpStatusCode.BadRequest);
    }
}