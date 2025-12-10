using Shorty.Dal.Entities;

namespace Shorty.Domain.Services.Abstractions;

public interface IAnalyticsService
{
    public Task RecalculateScoresAsinc();
    public Task<List<ShortyEntity>> GetTopPerformingAsync();
}
