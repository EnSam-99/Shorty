using Shorty.Domain.Models.Response;
using Shorty.Dal.Entities;
namespace Shorty.Domain.Services.Abstractions;

public interface IAnalyticsService
{
    Task RecalculateAllScoresAsync();
    Task<IList<ShortyEntity>> GetTopPerformingShortiesAsync(int limit = 10);
}
