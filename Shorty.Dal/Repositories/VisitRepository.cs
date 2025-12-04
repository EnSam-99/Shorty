using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;

namespace Shorty.Dal.Repositories;

public class VisitRepository(AppDbContext _db, IShortyUrlRepository<ShortyEntity> _shorties) : IVisitRepository
{
    public async Task AddVisitAsync(VisitEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "Visit entity cannot be null.");

        await _db.Visits.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task AddAutoVisitAsync(int count)
    {
        var shorties = await _shorties.GetAllShortiesAsync();
        if (shorties != null)
            foreach (var shorty in shorties)
                for (int i = 0; i < count; i++)
                {
                    var visit = new VisitEntity
                    {
                        ShortyId = shorty.Id,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _db.Visits.AddAsync(visit);
                }
        await _db.SaveChangesAsync();
    }
}
