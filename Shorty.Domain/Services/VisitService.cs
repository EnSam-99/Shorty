using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services;

public class VisitService(IVisitRepository _repo, IShortyUrlRepository<ShortyEntity> _shortyRepo, IAnalyticsRepository _analytics) : IVisitService
{
    public async Task AddAutoVisitAsync(int count)
    {
       await _repo.AddAutoVisitAsync(count);
    }

    public async Task AddVisitAsync(string shortCode)
    {
        if (string.IsNullOrWhiteSpace(shortCode))
        {
            throw new ArgumentException("ShortCode can't be null or empty.", nameof(shortCode));
        }
        var originalShorty = await _shortyRepo.GetByShortCodeAsync(shortCode);

        if (originalShorty == null)
        {
            return;
        }
        var visit = new VisitEntity { ShortyId = originalShorty.Id, CreatedDate = DateTime.UtcNow };
        originalShorty.LastClickAt = visit.CreatedDate;

        await _repo.AddVisitAsync(visit);
    }
}
