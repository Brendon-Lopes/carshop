using Carshop.Application.DTOs;

namespace Carshop.Application.Interfaces.User;

public interface IUserService
{
    Task<Domain.Models.User> GetById(Guid id);

    Task<IEnumerable<Domain.Models.User>> GetAll();

    Task<Domain.Models.User> Save(UserDTO user);

    Task<Domain.Models.User> Update(UserDTO user);
}