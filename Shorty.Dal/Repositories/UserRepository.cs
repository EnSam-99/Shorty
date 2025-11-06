using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Models;

namespace Shorty.Dal.Db.Repositories;

public class UserRepository : IUserRepository<UserEntity>
{
    readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;
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
