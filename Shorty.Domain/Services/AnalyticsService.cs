using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Domain.Models.Response;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext dbContext;

    public AnalyticsService(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<TopShortLinkDto>> GetTopShortLinksAsync(string email)
    {
        return await dbContext.Shorties
            .Where(s => s.User.Email == email)
            .Select(s => new TopShortLinkDto
            {
                ShortUrl = s.ShortCode,
                OriginalUrl = s.Url,
                Clicks = s.Visits.Count(),
                LastAccessedDate = s.Visits.Max(v => v.CreatedDate)
            })
            .OrderByDescending(s => s.Clicks)
            .Take(10)
            .ToListAsync();
    }
}
