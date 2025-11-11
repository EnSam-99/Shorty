using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class VisitRepository(AppDbContext _db) : IVisitRepository
{
    public async Task AddVisitAsync(VisitEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "Visit entity cannot be null.");

        await _db.Visits.AddAsync(entity);
        await _db.SaveChangesAsync();
    }
}
