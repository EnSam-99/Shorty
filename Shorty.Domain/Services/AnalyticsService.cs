using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;
using Shorty.Domain.Models.Response;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services;

public class AnalyticsService(IAnalyticsRepository _repo, AppDbContext dbContext) : IAnalyticsService
{
    public Task<List<ShortyEntity>> GetTopPerformingAsync()
    {
        var result = _repo.GetTopPerformingAsync();
        if (result is null)
        {
            throw new InvalidOperationException("No data found for top performing short URLs.");
        }
        return result;
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
			ShortCode = s.ShortCode,
            Url = s.Url,
			Clicks = s.Visits.Count,
			Score = s.Score,
            LastClickedAt = DateTime.UtcNow //todo: set actual last clicked date
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
				ShortCode = s.ShortCode,
				Url = s.Url,
				Clicks = s.Visits.Count,
				Score = s.Score
			})
			.ToListAsync();
	}

    public Task RecalculateScoresAsinc()
    {
        throw new NotImplementedException();
    }
}
