using Shorty.Dal.Entities;

namespace Shorty.Dal.Repositories.Abstractions;


public interface IShortyUrlRepository<T>
{
    public Task AddShortyAsync(T shortyModel);
    public Task UpdateAsync(ShortyEntity newShortCode);
    public Task<T> GetByIDAsync(int id);

    public Task<List<T>> GetAllShortiesAsync();
    public Task<bool> ExistsByUrlOrShortAsync(string origonalUrl, string shortUrl);
    public Task<bool> ExistsId(int id);
    public Task<string> GetOriginalShortUrlAsync(string shortCode);
    public Task<ShortyEntity> GetByShortCodeAsync(string shortCode);
    public Task<bool> IsShortCodeValidAsync(string shortCode);
    public Task DeleteIsNotValidShortyByName(string shortyName);
    public Task<bool> DeleteByIdAsync(int id);
    public Task<bool> DeleteByEmailAsync(string email);
}
