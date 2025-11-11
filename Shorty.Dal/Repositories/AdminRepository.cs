using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class AdminRepository : IAdminRepositoriy
{
    private readonly AppDbContext _db;
    public Task DeleteShortyByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<AdminUsersDto>> GetAllUsersForAdminAsync()
    {
        var summary = _db.Shorties
      .AsNoTracking()
      .GroupBy(s => new { s.UserId, s.User.Name, s.User.Email, s.User.IsAdmin })
      .Select(g => new AdminUsersDto
      {
          UserId = g.Key.UserId,
          UserName = g.Key.Name,
          Email = g.Key.Email,
          IsAdmin = g.Key.IsAdmin,
          LinksCount = g.Count(),
          TotalClicks = g.Sum(x => x.Clicks),
          ActiveLinks = g.Count(x => x.IsActive)
      })
      .OrderByDescending(x => x.TotalClicks)
      .ToListAsync();
        return summary;
    }
}
