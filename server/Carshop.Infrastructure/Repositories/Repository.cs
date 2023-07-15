using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;

namespace Carshop.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext _context;

    protected Repository(AppDbContext context)
    {
        _context = context;
    }

    public virtual TEntity? GetById(Guid id)
    {
        var query = _context.Set<TEntity>().Where(e => e.Id == id);

        return query.FirstOrDefault();
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        var query = _context.Set<TEntity>();

        return query.ToList();
    }

    public virtual TEntity Save(TEntity entity)
    {
        return _context.Set<TEntity>().Add(entity).Entity;
    }

    public virtual void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}