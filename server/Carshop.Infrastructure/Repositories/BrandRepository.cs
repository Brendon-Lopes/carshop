using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;

namespace Carshop.Infrastructure.Repositories;

public class BrandRepository : Repository<Brand>
{
    public BrandRepository(AppDbContext context) : base(context)
    {
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

    public override void Save(Brand entity)
    {
        _context.Brands.Add(entity);
    }

    public override void Update(Brand entity)
    {
        _context.Brands.Update(entity);
    }

    public override void Delete(Brand entity)
    {
        _context.Brands.Remove(entity);
    }
}