using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shorty.Dal.Db;
using Shorty.Dal.Models;

namespace Shorty.Dal.DbModel.Repositories;

public class UserRepository: IUserRepository<User>
{

    readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }
    public  Task<bool> ExistsByUser(string userName, string email) =>  _db.Users.AnyAsync(u => u.Name.Trim().ToLower() == userName.Trim().ToLower() || u.Email.Trim().ToLower() == email.Trim().ToLower());

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _db.Users
        .OrderByDescending(u => u.CreatedAt)
        .ToListAsync();

    }

    public async Task AddUserAsync(User user)
    {
        if (user.Id == Guid.Empty)
            user.Id = Guid.NewGuid();

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public Task UpdateUserAsync(User user, Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
