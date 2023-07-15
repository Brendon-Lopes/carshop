namespace Carshop.Domain.Interfaces;

public interface IUnitOfWork
{
    void Commit();
}