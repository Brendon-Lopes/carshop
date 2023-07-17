using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;

namespace Carshop.Infrastructure.Repositories;

public class CarRepository : Repository<Car>, ICarRepository
{
    public CarRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Car> Save(Car entity)
    {
        var created = await _context.Cars.AddAsync(entity);

        await _context.SaveChangesAsync();

        return created.Entity;
    }
}