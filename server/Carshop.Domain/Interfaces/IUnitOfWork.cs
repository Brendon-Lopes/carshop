namespace Carshop.Domain.Interfaces;

public interface IUnitOfWork
{
    Task Commit();
}