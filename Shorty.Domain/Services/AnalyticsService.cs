using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services;

public class AnalyticsService(IAnalyticsRepository _repo) : IAnalyticsService
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

    public async Task RecalculateScoresAsinc()
    {
        await _repo.RecalculateScores();
    }
}
