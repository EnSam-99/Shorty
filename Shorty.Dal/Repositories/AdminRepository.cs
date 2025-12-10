using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class AdminRepository(AppDbContext _db) : IAdminRepositoriy
{
    public async Task<List<UsersAdminDto>> GetAllUsersForAdminAsync()
    {
        return await _db.Shorties
            .AsNoTracking()
            .GroupBy(s => new
            {
                s.UserId,
                s.User.Name,
                s.User.Email,
                s.User.IsAdmin
            })
            .Select(g => new UsersAdminDto
            {
                UserId = g.Key.UserId,
                UserName = g.Key.Name,
                Email = g.Key.Email,
                IsAdmin = g.Key.IsAdmin,
                LinksCount = g.Count(),
                TotalClicks = 0, // todo: assign this 
                ActiveLinks = g.Count(x => x.IsActive)
            }).Take(50)
            .OrderByDescending(x => x.TotalClicks)
            .ToListAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        if (id == 0)
            throw new ArgumentException("Please enter a valid user ID.");

        var user = await _db.Users.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found.");

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteShortyByIdAsync(int shortyId)
    {
        if (shortyId == 0)
        {
            throw new ArgumentException("Please enter a valid user ID.");
        }

        var shorty = await _db.Shorties.FindAsync(shortyId);

        if (shorty == null)
            throw new KeyNotFoundException($"User with ID {shortyId} not found.");

        _db.Shorties.Remove(shorty);
        await _db.SaveChangesAsync();
    }

    public async Task<int> DeleteAllInactiveLinks()
    {

        var ids = await _db.Shorties
           .Where(s => !s.IsActive)
           .OrderByDescending(s => s.CreatedAt).Select(s => s.Id)
           .Take(1000)
           .ToListAsync();

        if (ids.Count == 0)
            return 0;

        var deleted = await _db.Shorties
           .Where(s => ids.Contains(s.Id))
           .ExecuteDeleteAsync();

        return deleted;
    }

    public async Task<int> DeactivateExpiredShortiesAsync()
    {
        var now = DateTime.UtcNow;

        var ids = await _db.Shorties
            .AsNoTracking()
            .Where(s => s.IsActive && s.ExpiredAt <= now)
            .OrderBy(s => s.ExpiredAt)
            .Select(s => s.Id)
            .Take(1000)
            .ToListAsync();

        if (ids.Count == 0)
            return 0;

        var updated = await _db.Shorties
            .Where(s => ids.Contains(s.Id) && s.IsActive)
            .ExecuteUpdateAsync(up => up
                .SetProperty(s => s.IsActive, s => false));

        return updated;
    }
}
