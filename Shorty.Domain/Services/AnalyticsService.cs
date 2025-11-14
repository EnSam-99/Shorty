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

	public async Task<List<TopShortLinkDto>> GetTopShortLinksByEmailAsync(string email, int limit = 10)
	{
		var shorties = await dbContext.Shorties
			.Include(s => s.Visits)
			.AsNoTracking()
			.Where(s => s.User.Email == email)
			.OrderByDescending(s => s.Score)
			.Take(limit)
			.ToListAsync();

		var result = shorties.Select(s => new TopShortLinkDto
		{
			ShortUrl = s.ShortCode,
			OriginalUrl = s.Url,
			Clicks = s.Visits.Count,
			Score = s.Score,
			LastAccessedDate = s.Visits
				.OrderByDescending(v => v.CreatedDate)
				.Select(v => (DateTime?) v.CreatedDate)
				.FirstOrDefault()

		}).ToList();

		return result;
	}

	public async Task RecalculateScoresAsync(int batchSize = 100)
	{
		var shorties = await dbContext.Shorties
			.Include(s => s.Visits)
			.AsTracking()
			.OrderBy(s => s.ShortyUpdatedDate ?? DateTime.MinValue)
			.Take(batchSize)
			.ToListAsync();

		if (shorties.Count == 0)
			return;

		var now = DateTime.UtcNow;

		foreach (var s in shorties)
		{
			var ageInDays = (decimal)(now - s.CreatedAt).TotalDays;

			if (ageInDays <= 0)
				ageInDays = 1;

			var clicks = s.Visits.Count;
			s.Score = (clicks / ageInDays) + (s.IsActive ? 10 : 0);
			s.ShortyUpdatedDate = now;
		}
		await dbContext.SaveChangesAsync();
	}

	public async Task<List<TopShortLinkDto>> GetTopShortLinksAsync(int limit)
	{
		return await dbContext.Shorties
			.Include(s => s.Visits)
			.AsNoTracking()
			.OrderByDescending(s => s.Score)
			.Take(limit)
			.Select(s => new TopShortLinkDto
			{
				ShortUrl = s.ShortCode,
				OriginalUrl = s.Url,
				Clicks = s.Visits.Count,
				Score = s.Score
			})
			.ToListAsync();
	}
}
