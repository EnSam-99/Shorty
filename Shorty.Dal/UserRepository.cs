using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shorty.Dal.Models;

namespace Shorty.Dal;

public class UserRepository
{

    readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }


    public async Task<List<User>> GetAllUsersAsync()
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

}
