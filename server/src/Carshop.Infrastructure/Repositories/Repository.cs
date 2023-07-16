using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;

namespace Carshop.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public virtual Task<IEnumerable<TEntity>> GetAll()
    {
        return Task.FromResult(_context.Set<TEntity>().AsEnumerable());
    }

    public virtual async Task<TEntity> Save(TEntity entity)
    {
        var created = await _context.Set<TEntity>().AddAsync(entity);

        await _context.SaveChangesAsync();

        return created.Entity;
    }

    public virtual async Task<TEntity> Update(TEntity entity)
    {
        var result = _context.Set<TEntity>().Update(entity);

        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public virtual async Task Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);

        await _context.SaveChangesAsync();
    }
}