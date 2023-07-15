using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;

namespace Carshop.Infrastructure.Repositories;

public class BrandRepository : Repository<Brand>
{
    private readonly IUnitOfWork _unitOfWork;

    public BrandRepository(AppDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _unitOfWork = unitOfWork;
    }

    public override Brand? GetById(Guid id)
    {
        var query = _context.Brands.Where(b => b.Id == id);

        return query.FirstOrDefault();
    }

    public override IEnumerable<Brand> GetAll()
    {
        var query = _context.Brands;

        return query.ToList();
    }

    public override Brand Save(Brand entity)
    {
        var created = _context.Brands.Add(entity);

        _unitOfWork.Commit();

        return created.Entity;
    }

    public override void Update(Brand entity)
    {
        _context.Brands.Update(entity);

        _unitOfWork.Commit();
    }

    public override void Delete(Brand entity)
    {
        _context.Brands.Remove(entity);

        _unitOfWork.Commit();
    }
}