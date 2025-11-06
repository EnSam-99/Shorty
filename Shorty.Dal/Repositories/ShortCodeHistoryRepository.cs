using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Entities;

namespace Shorty.Dal.Db.Repositories;

public class ShortCodeHistoryRepository(AppDbContext _context) : IShortCodeHistoryRepository<ShortyHistoryEntity>
{
    public async Task AddHistoryAsync(ShortyHistoryEntity history)
    {
        await _context.Histories.AddAsync(history);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> ExistsNewShortCodeAsync(string newShortCode) =>
      await _context.Histories.AnyAsync(n => n.NewShortUrl.Trim() == newShortCode.Trim());

    public async Task<bool> ExistsOldShortCodeAsync(string oldShortCode) =>
        await _context.Histories.AnyAsync(n => n.OldShortUrl.Trim() == oldShortCode.Trim());

    public async Task<IEnumerable<ShortyHistoryEntity>> GetAllHistoryAsync()
    {
        var historyList = await _context.Histories.OrderByDescending(x => x.Id).ToListAsync();

        return historyList;
    }
}
