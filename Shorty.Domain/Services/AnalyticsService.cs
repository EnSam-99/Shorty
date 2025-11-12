using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;
using Shorty.Domain.Models.Response;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services;
public class AnalyticsService(IAnalyticsRepository repository): IAnalyticsService
{
    public async Task RecalculateAllScoresAsync()
    {
        var shorties = await repository.GetAllShortiesAsync();
        foreach (var sh in shorties )
        {
            var ageInDays = (DateTime.UtcNow - sh.CreatedAt).TotalDays;
            if (ageInDays <= 0)
            {
                ageInDays = 1;
            }

            var score =(sh.Clicks / ageInDays) +(sh.IsActive ? 10 : 0);
            sh.Score = (decimal)score;
        }
    }

    public async Task<IList<ShortyEntity>> GetTopPerformingShortiesAsync(int limit = 10)
    {
        return await repository.GetTopPerformingShortiesAsync(limit);
    }
}
