
namespace Shorty.Domain.Services.Abstractions;

public interface IUrlService<T>
{
    public Task<T> CreateShortAsync(string originalUrl, int userId, int? expirationHours = null);
    public Task UpdateShortCodAsync(int id, string shortyCode);
    public Task<IEnumerable<T>> GetAllShortCodesAsync();
    public Task<string> GetOriginalUrlAsync(string shortCode);
}
