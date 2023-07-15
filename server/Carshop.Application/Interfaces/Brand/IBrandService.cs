using Carshop.Application.DTOs;

namespace Carshop.Application.Interfaces.Brand;

public interface IBrandService
{
    Domain.Models.Brand GetById(Guid id);

    IEnumerable<Domain.Models.Brand> GetAll();

    void Save(BrandDTO brand);
}