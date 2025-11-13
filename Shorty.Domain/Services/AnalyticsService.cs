using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Domain.Models;
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

    public async Task<UserAnalyticsDto> GetUserAnalyticsAsync(string email)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return null;

        var shorties = await dbContext.Shorties
            .Where(s => s.UserId == user.Id)
            .Include(s => s.Visits)
            .ToListAsync();

        if (shorties.Count == 0)
            return new UserAnalyticsDto(
                TotalShortenedUrls: 0,
                TotalClicks: 0,
                AverageClicksPerShorty: 0m,
                MostPopularShortCode: null,
                MostPopularClicks: null
            );

        var totalShorties = shorties.Count;
        var totalClicks = shorties.Sum(s => s.Visits.Count);
        var avgClicks = totalShorties > 0 ? (decimal)totalClicks / totalShorties : 0m;
        
        var mostPopular = shorties
            .OrderByDescending(s => s.Visits.Count)
            .First();

        var mostPopularDto = new MostPopularDto(
            ShortCode: mostPopular.ShortCode,
            Clicks: mostPopular.Visits.Count
        );

        return new UserAnalyticsDto(
            TotalShortenedUrls: totalShorties,
            TotalClicks: totalClicks,
            AverageClicksPerShorty: Math.Round(avgClicks, 2),
            MostPopularShortCode: mostPopular.ShortCode,
            MostPopularClicks: mostPopularDto
        );
    }

}
