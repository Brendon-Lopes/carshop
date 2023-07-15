using Carshop.Domain.Interfaces;
using Carshop.Infrastructure.Context;

namespace Carshop.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}