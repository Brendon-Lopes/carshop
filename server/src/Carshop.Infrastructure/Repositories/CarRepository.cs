using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carshop.Infrastructure.Repositories;

public class CarRepository : Repository<Car>, ICarRepository
{
    public CarRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IList<Car>> GetFilteredCars(string name, string brandName)
    {
        var nameLowerCase = name.ToLower();

        var filteredCars = _context.Cars
            .Where(c => c.Name.ToLower().Contains(nameLowerCase)
                        || c.Model.ToLower().Contains(nameLowerCase)
                        || c.Brand.Name.ToLower().Contains(nameLowerCase))
            .Where(c => string.IsNullOrEmpty(brandName) || c.Brand.Name.ToLower() == brandName.ToLower())
            .Include(c => c.Brand);

        return await filteredCars.ToListAsync();
    }

    public override async Task<Car> Save(Car entity)
    {
        var created = await _context.Cars.AddAsync(entity);

        var brand = await _context.Brands.FindAsync(entity.BrandId);

        await _context.SaveChangesAsync();

        var carResult = created.Entity;

        carResult.Brand = brand!;

        return carResult;
    }
}