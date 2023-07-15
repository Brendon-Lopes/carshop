using AutoMapper;
using Carshop.Application.DTOs;
using Carshop.Application.Interfaces.Brand;
using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;

namespace Carshop.Application.Services;

public class BrandService : IBrandService
{
    private readonly IRepository<Brand> _brandRepository;
    private readonly IMapper _mapper;

    public BrandService(IRepository<Brand> brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    public Brand GetById(Guid id)
    {
        var brand = _brandRepository.GetById(id);

        if (brand is null) throw new Exception("Brand not found");

        return brand;
    }

    public IEnumerable<Brand> GetAll()
    {
        return _brandRepository.GetAll();
    }

    public void Save(BrandDTO brand)
    {
        var newBrand = _mapper.Map<Brand>(brand);

        _brandRepository.Save(newBrand);
    }
}