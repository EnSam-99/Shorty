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

	public async Task RecalculateScoresAsync()
	{
		var shorties = await dbContext.Shorties
			.AsNoTracking()
			.Select(s => new
			{
				s.Id,
				s.CreatedAt,
				s.IsActive
			})
			.ToListAsync();

		var clickCounts = await dbContext.ShortyClickCounts
			.AsNoTracking()
			.ToDictionaryAsync(c => c.ShortyId, c => c.ClickCount);

		foreach (var s in shorties)
		{
			clickCounts.TryGetValue(s.Id, out var clicks);

			decimal ageInDays = (decimal)(DateTime.UtcNow - s.CreatedAt).TotalDays;
			if (ageInDays <= 0)
				ageInDays = 1;

			var score = (clicks / ageInDays) + (s.IsActive ? 10 : 0);

			await dbContext.Database.ExecuteSqlInterpolatedAsync(
				$"UPDATE \"Shorties\" SET \"Score\" = {score} WHERE \"Id\" = {s.Id}");
		}
	}

	public async Task<List<TopShortLinkDto>> GetTopShortLinksAsync(int limit)
	{
		Dictionary<int, long> clickCounts = await dbContext.ShortyClickCounts
			.AsNoTracking()
			.ToDictionaryAsync(c => c.ShortyId, c => c.ClickCount);

		var shorties = await dbContext.Shorties
			.AsNoTracking()
			.OrderByDescending(s => s.Score)
			.Take(limit)
			.Select(s => new
			{
				s.Id,
				s.ShortCode,
				s.Url,
				s.Score
			})
			.ToListAsync();

		List<TopShortLinkDto> result = new List<TopShortLinkDto>(shorties.Count);

		foreach (var s in shorties)
		{
			clickCounts.TryGetValue(s.Id, out var count);

			result.Add(new TopShortLinkDto
			{
				ShortUrl = s.ShortCode,
				OriginalUrl = s.Url,
				Clicks = count,
				Score = s.Score
			});
		}

		return result;
	}


}
