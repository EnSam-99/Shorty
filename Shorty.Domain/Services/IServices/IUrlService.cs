using Shorty.Dal.Models;
using Shorty.Models;

namespace Shorty.Services.IServices;

public interface IUrlService<T>
{
   public Task<T> CreateShortAsync(string originalUrl, int userId);
    public Task UpdateShortCodAsync(int id, string shortyCode);
    public Task<IEnumerable<T>> GetAllShortCodesAsync();
   public Task<string> GetOriginalUrlAsync(string shortCode);
}
