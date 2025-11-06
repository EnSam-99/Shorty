using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Db.Repositories;

public class ShortyUrlRepository : IShortyUrlRepository<ShortyEntity>
{
    readonly AppDbContext _context;
    public ShortyUrlRepository(AppDbContext context) => _context = context;
    public async Task AddShortyAsync(ShortyEntity shortyModel)
    {
        await _context.Shorties.AddAsync(shortyModel);
        await _context.SaveChangesAsync();
    }
    public Task<bool> ExistsByUrlOrShortAsync(string origonalUrl, string shortUrl) =>
        _context.Shorties.AnyAsync(u => u.Url.Trim() == origonalUrl.Trim()
        || u.ShortCode.Trim() == shortUrl.Trim());

    public async Task<bool> ExistsId(int id)
    {
        var isExists = await _context.Shorties.AnyAsync(u => u.Id == id);
        return isExists;
    }
    public async Task<List<ShortyEntity>> GetAllShortyAsync()
    {
        var list = await _context.Shorties.OrderByDescending(s => s.Id).ToListAsync();
        return list;
    }
    public async Task<ShortyEntity> GetByIDAsync(int id)
    {
        var shorty = await _context.Shorties.FirstOrDefaultAsync(s => s.Id == id);
        if (shorty == null)
            throw new KeyNotFoundException($"Short code '{shorty}' not found.");

        return shorty;
    }
    public async Task<string> GetOriginalShortUrlAsync(string shortCode)
    {
        if (string.IsNullOrWhiteSpace(shortCode))
            throw new ArgumentException("ShortCode is empty.", nameof(shortCode));
       
        var url = await _context.Shorties.AsNoTracking().FirstOrDefaultAsync(s => s.ShortCode == shortCode);
        if (url == null)
            throw new KeyNotFoundException($"Url '{shortCode}' not found.");

        return url.Url;
    }
    public async Task UpdateAsync(ShortyEntity shortyModel)
    {
        if (shortyModel is null)
            throw new ArgumentNullException(nameof(shortyModel));

        if (shortyModel.Id == 0)
            throw new ArgumentException("Id is empty.", nameof(shortyModel.Id));

        if (string.IsNullOrWhiteSpace(shortyModel.ShortCode))
            throw new ArgumentException("ShortCode is empty.", nameof(shortyModel.ShortCode));

        if(await IsShortCodeValidateAsync(shortyModel.ShortCode) == false)
            throw new InvalidOperationException("ShortCode is not valid or expired.");

        await _context.Shorties.Where(x => x.Id == shortyModel.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.ShortCode, shortyModel.ShortCode)
        .SetProperty(l => l.UpdatedAt, DateTime.UtcNow));
    }

    public async Task<bool> IsShortCodeValidateAsync(string shortCode)
    {
        var timeNow = DateTime.UtcNow;
        var isValid = await _context.Shorties
            .AnyAsync(s => s.ShortCode == shortCode && timeNow < s.ExpiredAt);
        return isValid;
    }

    public async Task DeleteIsNotValidShortyByName(string shortyName)
    {
        if(string.IsNullOrWhiteSpace(shortyName))
            throw new ArgumentException("ShortyName is empty.", nameof(shortyName));

        if (!await _context.Shorties.AnyAsync())
        {
            throw new InvalidOperationException("Shorties list is empty.");
        }
        if (!await IsShortCodeValidateAsync(shortyName))
        {
            await _context.Shorties.Where(s => s.ShortCode == shortyName).ExecuteDeleteAsync();
        }
    }
}