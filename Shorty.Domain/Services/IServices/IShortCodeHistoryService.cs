using Shorty.Domain.Models;

namespace Shorty.Domain.Services.IServices;

public interface IShortCodeHistoryService
{
    public Task CreateHistoryAsync(int id, string? oldShortCode, string newShortCode);
    public Task<IEnumerable<ShortHistoryDto>> GetAllShortsHistoryAsync();
}
