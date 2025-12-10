using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class AnalyticsRepository(AppDbContext _db) : IAnalyticsRepository
{
    public async Task<List<ShortyEntity>> GetTopPerformingAsync()
    {
        return await _db.Shorties.OrderByDescending(s => s.Score)
                            .Take(500)
                            .ToListAsync();
    }

    public async Task<int> GetTotalVisitsByIdAsync(int shortyId)
    {
        if (shortyId <= 0)
        {
            throw new ArgumentException("ShortyId must be greater than zero.", nameof(shortyId));
        }

        var totalVisits = await _db.Visits.CountAsync(v => v.ShortyId == shortyId);

        var shorty = await _db.Shorties.FirstOrDefaultAsync(s => s.Id == shortyId);

        if (shorty == null)
        {
            throw new InvalidOperationException($"Shorty with ID={shortyId} not found.");
        }

        await _db.SaveChangesAsync();

        return totalVisits;
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
        if (shortyId <= 0)
            throw new ArgumentException("ShortyId must be greater than zero.", nameof(shortyId));

        var isActive = await _db.Shorties.AnyAsync(s => s.Id == shortyId && s.ExpiredAt > s.CreatedAt && s.Clicks > 0);

        return isActive;
    }

    public async Task RecalculateScores()
    {
        var now = DateTime.UtcNow;

        var shorties = await _db.Shorties
            .Where(s => s.IsActive && s.ExpiredAt > now &&
                 s.ScoreUpdatedAt < s.LastClickAt
            )
            .OrderByDescending(s => s.ScoreUpdatedAt)
            .Take(500)
            .ToListAsync();

        if (shorties.Count == 0)
            return;

        foreach (var shorty in shorties)
        {
            var age = now - shorty.CreatedAt;
            var clicks = shorty.Clicks;
            var isActive = await GetActiveShoryLinkAsync(shorty.Id);

            shorty.ScoreUpdatedAt = now;

            if (!isActive)
            {
                continue;
            }

            var safeAge = Math.Max(1, (int)age.TotalDays);
            shorty.Score = ((decimal)clicks / safeAge) + 10;
        }

        await _db.SaveChangesAsync();
    }

}
