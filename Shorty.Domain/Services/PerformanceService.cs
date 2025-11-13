using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Entities;

namespace Shorty.Domain.Services;

public class PerformanceService(IShortyUrlRepository<ShortyEntity> _shortyUrlRepository)
{
    public async Task RecalculateScoresAsync()
    {
        var now = DateTime.UtcNow;
        var shorties = await _shortyUrlRepository.GetAllWithVisitsAsync();

        foreach (var shorty in shorties)
        {
            var ageInDays = (now - shorty.CreatedAt).TotalDays;
            if (ageInDays < 1) ageInDays = 1;

            var clicks = shorty.Visits.Count;
            shorty.Clicks = clicks;

            shorty.Score = (decimal)(clicks / ageInDays) + (shorty.IsActive ? 10 : 0);
        }

        await _shortyUrlRepository.SaveChangesAsync();
    }

    public async Task<List<ShortyEntity>> GetTopShortiesAsync(int topN)
    {
        var topShorties = await _shortyUrlRepository.GetTopByScoreAsync(topN);
        return topShorties;
    }
}

