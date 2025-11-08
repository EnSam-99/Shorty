using Shorty.Domain.Models.Response;

namespace Shorty.Domain.Services.Abstractions;

public interface IAnalyticsService
{
    Task<List<TopShortLinkDto>> GetTopShortLinksAsync(string email);
}
