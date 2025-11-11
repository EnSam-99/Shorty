using Shorty.Dal.Entities;

namespace Shorty.Dal.Repositories.Abstractions;

public interface IAnalyticsRepository
{
    public Task<int> GetTotalVisitsByIdAsync(int shortyId);
    public Task<int> GetShortLinkAgeAsync(int shortyId);
    public Task<bool> GetActiveShoryLinkAsync(int shortyId);
    public Task<decimal> GetShortLinkScoreAsync(int id);
    public Task RecalculateScores();
    public Task<List<ShortyEntity>> GetTopPerformingAsync();
}
