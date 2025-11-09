using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;
using System;

namespace Shorty.Dal.Repositories;

public class AnalyticsRepository(AppDbContext _db, ShortyUrlRepository _repository) : IAnalyticsRepository
{
    public async Task<List<ShortyEntity>> GetTopPerformingAsync()
    {
        var shorties = await _repository.GetAllShortyAsync();
        return shorties.Take(10).ToList();
    }

    public async Task<int> GetTotalVisitsByIdAsync(int shortyId)
    {
        if (shortyId <= 0)
            throw new ArgumentException("ShortyId must be greater than zero.", nameof(shortyId));

        var totalVisits = _db.Visits.Count(v => v.ShortyId == shortyId);
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

        var visitsCount = await GetTotalVisitsByIdAsync(shortyId);

        if (shortyId <= 0)
            throw new ArgumentException("ShortyId must be greater than zero.", nameof(shortyId));

        var isActive = await _db.Shorties.AnyAsync(s => s.Id == shortyId && s.ExpiredAt > s.CreatedAt && visitsCount > 0);

        return isActive;
    }

    public async Task<decimal> GetShortLinkScoreAsync(int id)
    {
        var visitsCount = await GetTotalVisitsByIdAsync(id);
        var age = await GetShortLinkAgeAsync(id);
        var isActive = await GetActiveShoryLinkAsync(id);
        decimal score;

        if (id <= 0)
        {
            score = 0;
            return score;
        }
       
        if (visitsCount == 0 && age == 100 && !isActive)
        {
            score = 0;
            return score;
        }

        if (age == 0)
        {
            age = 1;
        }

        score = (decimal)(visitsCount / age) + (isActive ? 10 : 0);

        return score;
    }

    public async Task RecalculateScores()
    {
        var shorties = await _repository.GetAllShortyAsync();
        foreach (var shorty in shorties)
        {
            var score = await GetShortLinkScoreAsync(shorty.Id);
            shorty.Score = score;
        }
        await _db.SaveChangesAsync();
    }
}
