using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class AnalyticsRepository(AppDbContext _db, IShortyUrlRepository<ShortyEntity> _repository) : IAnalyticsRepository
{
    public async Task<List<ShortyEntity>> GetTopPerformingAsync()
    {
        return await _repository.GetAllShortyAsync();
    }

    public async Task<int> GetTotalVisitsByIdAsync(int shortyId)
    {
        if (shortyId <= 0)
            throw new ArgumentException("ShortyId must be greater than zero.", nameof(shortyId));

        var totalVisits = _db.Visits.CountAsync(v => v.ShortyId == shortyId);
        return await totalVisits;
    }

    public async Task<int> GetShortLinkAgeAsync(int shortyId)
    {
        if (shortyId <= 0)
            throw new ArgumentException("ShortyId must be greater than zero.", nameof(shortyId));

        var entity = await _db.Shorties.FirstOrDefaultAsync(s => s.Id == shortyId);

        if (entity == null)
            throw new KeyNotFoundException($"Shorty with ID '{shortyId}' not found;");

        var age = DateTime.UtcNow - entity.CreatedAt;

        return (int)age.TotalDays;
    }

    public async Task<bool> GetActiveShoryLinkAsync(int shortyId)
    {

        var visitsCount = await GetTotalVisitsByIdAsync(shortyId);

        if (shortyId <= 0)
            throw new ArgumentException("ShortyId must be greater than zero.", nameof(shortyId));

        var isActive = await _db.Shorties.AnyAsync(s => s.Id == shortyId && s.ExpiredAt > s.CreatedAt && visitsCount > 0);

        return isActive;
    }

    public async Task RecalculateScores()
    {
        var shorties = await _db.Shorties
                             .OrderByDescending(s => s.Score)
                             .ToListAsync();

        foreach (var shorty in shorties)
        {
            var visitsCount = await GetTotalVisitsByIdAsync(shorty.Id);
            var age = await GetShortLinkAgeAsync(shorty.Id);
            var isActive = await GetActiveShoryLinkAsync(shorty.Id);
            if (visitsCount == 0 && age == 100 && !isActive)
            {
                return;
            }
            if (age == 0)
            {
                age = 1;
            }
            var score = (decimal)(visitsCount / age) + (isActive ? 10 : 0);
            shorty.Score = score;
        }
        await _db.SaveChangesAsync();
    }
}
