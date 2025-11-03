using Shorty.Domain.Models.Request;
using Shorty.Domain.Models.Response;

namespace Shorty.Domain.Abstraction;

public interface IShortyUrlService
{
    Task<ShortyCreateDto> CreateShortyAsync(ShortyCreateRequestModel model);
	Task<List<ShortyListDto>> GetShortiesByEmailAsync(string email);
	Task<string?> ResolveAsync(string shortCode);
	Task<ShortyStatsDto?> GetStatsAsync(string shortCode);
	Task<string?> ResolveAndTrackAsync(string shortCode);
	Task<List<(string ShortCode, int Clicks)>> GetTopAsync(int take);
}
