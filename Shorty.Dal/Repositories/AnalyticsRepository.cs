using System.Collections;
using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class AnalyticsRepository(AppDbContext context) : IAnalyticsRepository
{
    public async Task<IList<ShortyEntity>> GetAllShortiesAsync()
    {
        return await context.Shorties.ToListAsync();

    }

    public async Task<IList<ShortyEntity>> GetTopPerformingShortiesAsync(int limit = 10)
    {
        return await context.Shorties
            .Where(s => s.IsActive)
            .OrderByDescending(s => s.Score)
            .Take(limit)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
    
}
