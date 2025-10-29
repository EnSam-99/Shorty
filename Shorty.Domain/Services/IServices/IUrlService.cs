using Shorty.Dal.Models;

namespace Shorty.Services.IServices;

public interface IUrlService
{

    public Task<ShortyModel> CreateShortAsync(string originalUrl, Guid userId);

}
