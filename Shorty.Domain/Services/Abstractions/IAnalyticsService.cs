using Shorty.Dal.Entities;
using Shorty.Domain.Models.Response;

namespace Shorty.Domain.Services.Abstractions;

public interface IAnalyticsService
{
    public Task RecalculateScoresAsinc();
    public Task<List<ShortyEntity>> GetTopPerformingAsync();
    Task<List<TopShortLinkDto>> GetTopShortLinksByEmailAsync(string email, int limit = 10);
    Task RecalculateScoresAsync(int batchSiza);
	Task<List<TopShortLinkDto>> GetTopShortLinksAsync(int limit);

}
