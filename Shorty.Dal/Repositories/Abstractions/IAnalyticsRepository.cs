using Shorty.Dal.Entities;

namespace Shorty.Dal.Repositories.Abstractions;

public interface IAnalyticsRepository
{
    Task<IList<ShortyEntity>> GetAllShortiesAsync();
    Task<IList<ShortyEntity>> GetTopPerformingShortiesAsync(int limit = 10);
    Task SaveChangesAsync();


}
