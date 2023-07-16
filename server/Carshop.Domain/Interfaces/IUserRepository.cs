using Carshop.Domain.Models;

namespace Carshop.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email);
}