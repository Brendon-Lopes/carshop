using Carshop.Domain.Interfaces;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Carshop.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    protected UserRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<User?> GetById(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public override async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public override async Task<User> Save(User entity)
    {
        var created = await _context.Users.AddAsync(entity);

        await _context.SaveChangesAsync();

        return created.Entity;
    }

    public override async Task<User> Update(User entity)
    {
        var updatedUser = _context.Users.Update(entity);

        await _context.SaveChangesAsync();

        return updatedUser.Entity;
    }
}