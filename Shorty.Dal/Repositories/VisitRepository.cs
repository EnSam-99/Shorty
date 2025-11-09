using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
