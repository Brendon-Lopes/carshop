namespace Carshop.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity GetById(Guid id);
    IEnumerable<TEntity> GetAll();
    void Save(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}