using Carshop.Application.DTOs;

namespace Carshop.Application.Interfaces.Brand;

public interface IBrandService
{
    Task<Domain.Models.Brand> GetById(Guid id);

    Task<IEnumerable<BrandResponse>> GetAll();

    Task<BrandResponse> Save(BrandDTO brand);

    Task CheckIfBrandExists(Guid brandId);
}