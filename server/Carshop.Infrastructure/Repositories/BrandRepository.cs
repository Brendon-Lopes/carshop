using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carshop.Infrastructure.Repositories;

public class BrandRepository : Repository<Brand>
{
    public BrandRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Brand?> GetById(Guid id)
    {
        return await _context.Brands.FindAsync(id);
    }

    public override async Task<IEnumerable<Brand>> GetAll()
    {
        return await _context.Brands.ToListAsync();
    }

    public override async Task<Brand> Save(Brand entity)
    {
        var created = await _context.Brands.AddAsync(entity);

        await _context.SaveChangesAsync();

        return created.Entity;
    }

    public override async Task<Brand> Update(Brand entity)
    {
        var updatedBrand = _context.Brands.Update(entity);

        await _context.SaveChangesAsync();

        return updatedBrand.Entity;
    }
}