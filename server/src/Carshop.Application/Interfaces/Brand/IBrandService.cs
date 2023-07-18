using Carshop.Application.DTOs;

namespace Carshop.Application.Interfaces.Brand;

public interface IBrandService
{
    Task<Domain.Models.Brand> GetById(Guid id);

    Task<IEnumerable<Domain.Models.Brand>> GetAll();

    Task<Domain.Models.Brand> Save(BrandDTO brand);

    Task CheckIfBrandExists(Guid brandId);
}