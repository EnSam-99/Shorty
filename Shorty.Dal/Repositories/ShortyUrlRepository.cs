using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class ShortyUrlRepository(AppDbContext _context) : IShortyUrlRepository<ShortyEntity>
{
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
    public async Task<List<ShortyEntity>> GetAllShortyAsync(int count = 10)
    {
        return await _context.Shorties
                             .OrderByDescending(s => s.Score)
                             .Take(count)
                             .ToListAsync();
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

    public async Task<ShortyEntity> GetByShortCodeAsync(string shortCode)
    {
        if (string.IsNullOrWhiteSpace(shortCode))
            throw new ArgumentException("ShortCode is empty.", nameof(shortCode));

        var shorty = await _context.Shorties.FirstOrDefaultAsync(s => s.ShortCode == shortCode);

        if (shorty == null)
            throw new KeyNotFoundException($"Short code '{shortCode}' not found.");

        return shorty;
    }

    public async Task UpdateAsync(ShortyEntity shortyModel)
    {
        if (shortyModel is null)
            throw new ArgumentNullException(nameof(shortyModel));

        if (shortyModel.Id == 0)
            throw new ArgumentException("Id is empty.", nameof(shortyModel.Id));

        if (string.IsNullOrWhiteSpace(shortyModel.ShortCode))
            throw new ArgumentException("ShortCode is empty.", nameof(shortyModel.ShortCode));


        await _context.Shorties.Where(x => x.Id == shortyModel.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.ShortCode, shortyModel.ShortCode)
        .SetProperty(l => l.UpdatedAt, DateTime.UtcNow));
    }

    public async Task<bool> IsShortCodeValidAsync(string shortCode)
    => await _context.Shorties
            .AnyAsync(s => s.ShortCode == shortCode && DateTime.UtcNow < s.ExpiredAt);

    public async Task DeleteIsNotValidShortyByName(string shortyName)
    {
        if (string.IsNullOrWhiteSpace(shortyName))
            throw new ArgumentException("ShortyName is empty.", nameof(shortyName));

        if (!await _context.Shorties.AnyAsync())
            throw new InvalidOperationException("Shorties list is empty.");

        if (!await IsShortCodeValidAsync(shortyName))
            await _context.Shorties.Where(s => s.ShortCode == shortyName).ExecuteDeleteAsync();
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id must be greater than zero.", nameof(id));

        if (!await _context.Shorties.AnyAsync())
            throw new InvalidOperationException("Shorties list is empty.");

        var shortyForDlelete = await _context.Shorties.Where(s => s.Id == id).ExecuteDeleteAsync();
        return shortyForDlelete != 0;
    }

    public async Task<bool> DeleteByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("email is empty. ", nameof(email));

        if (!await _context.Shorties.AnyAsync())
            throw new InvalidOperationException("Shorties list is empty.");

        var deleteByEmail = await _context.Shorties.Where(u => u.User.Email == email).ExecuteDeleteAsync();

        return deleteByEmail != 0;
    }

    public async Task<bool> DeactivateByIdAsync(string shortyName)
    {        
        if (string.IsNullOrWhiteSpace(shortyName))
            throw new ArgumentException("ShortyName is empty.", nameof(shortyName));

        if (!await IsShortCodeValidAsync(shortyName))
            throw new InvalidOperationException($"Shotry is not valid {shortyName}");

        var deactivate = await _context.Shorties.Where(s => s.ShortCode == shortyName && s.Visits.Count == 0 && !s.IsActive && DateTime.UtcNow - s.CreatedAt >= TimeSpan.FromDays(100)).ExecuteUpdateAsync(s => s.SetProperty(p => p.IsActive, false));

        return deactivate != 0;
    }
}