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

    public virtual void Save(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
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