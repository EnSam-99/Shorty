using Shorty.Dal.Entities;

namespace Shorty.Dal.Db.IRepositories;

public interface IShortyUrlRepository<T>
{
    public Task AddShortyAsync(T shortyModel);
    public Task UpdateAsync(ShortyEntity newShortCode);
    public Task<T> GetByIDAsync(int id);
    public Task<List<T>> GetAllShortyAsync();
    public Task<bool> ExistsByUrlOrShortAsync(string origonalUrl, string shortUrl);
    public Task<bool> ExistsId(int id);
    public Task<string> GetOriginalShortUrlAsync(string shortCode);
    public Task<bool> IsShortCodeValidAsync(string shortCode);
    public Task DeleteIsNotValidShortyByName(string shortyName);
    public Task<List<ShortyEntity>> GetAllWithVisitsAsync();
    public Task<List<ShortyEntity>> GetTopByScoreAsync(int limit);
    public Task SaveChangesAsync();

}
