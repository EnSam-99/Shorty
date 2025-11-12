using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class AnalyticsRepository(AppDbContext _db, IShortyUrlRepository<ShortyEntity> _repository) : IAnalyticsRepository
{
    public async Task<List<ShortyEntity>> GetTopPerformingAsync()
    {
        return await _repository.GetTopShortiesByScoreAsync();
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

        var nowTime = DateTime.UtcNow;
        var shorties = await _db.Shorties.Where(s =>s.ScoreUpdatedAt < s.LastClickAt || s.ScoreUpdatedAt < nowTime.AddMinutes(-15) || s.Clicks !=0)
                             .OrderByDescending(s => s.ScoreUpdatedAt)
                             .ToListAsync();

        if (shorties == null) return;  

        foreach (var shorty in shorties)
        {
            if (shorty == null)
                throw new KeyNotFoundException($"Shorty '{shorty}' not found;");

            var age = DateTime.UtcNow - shorty.CreatedAt;
            var clicks = shorty.Clicks;            
            var isActive = await GetActiveShoryLinkAsync(shorty.Id);
            if (isActive)
            {
                int safeAge = Math.Max(1, (int)age.TotalDays);

                var score = ((decimal)clicks / safeAge) + (isActive ? 10 : 0);
               
                    shorty.Score = score;
                    shorty.ScoreUpdatedAt = nowTime;
                

            }


        }
        await _db.SaveChangesAsync();
    }
}
