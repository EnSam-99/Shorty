using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class UserRepository(AppDbContext _db) : IUserRepository<UserEntity>
{
    public Task<bool> ExistsByUser(string userName, string email) =>
        _db.Users.AnyAsync(u => u.Name.Trim().ToLower() == userName.Trim().ToLower()
        || u.Email.Trim().ToLower() == email.Trim().ToLower());

    public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
    {
        return await _db.Users
        .OrderByDescending(u => u.CreatedAt)
        .ToListAsync();
    }
    public async Task AddUserAsync(UserEntity user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }
}
