using Shorty.Dal.Entities;
using Shorty.Domain.Models.Response;

namespace Shorty.Domain.Services.Abstractions;

public interface IAnalyticsService
{
    public Task RecalculateScoresAsinc();
    public Task<List<ShortyEntity>> GetTopPerformingAsync();
}
