namespace Shorty.Dal.Repositories.Abstractions;

public interface IShortCodeHistoryRepository<T>
{
    public Task AddHistoryAsync(T history);
    public Task<bool> ExistsOldShortCodeAsync(string oldShortCode);
    public Task<bool> ExistsNewShortCodeAsync(string newShortCode);
    public Task<IEnumerable<T>> GetAllHistoryAsync();
}
