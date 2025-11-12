using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Repositories.Abstractions;
using Shorty.Dal.Entities;

namespace Shorty.Dal.Repositories;

public class ClickDetailRepository(AppDbContext dbContext) : IClickDetailRepository
{
    public async Task<IEnumerable<ClickDetailEntity>> GetByShortyIdAsync(int shortyId)
    {
        return await dbContext.ClickDetails
            .Where(c => c.ShortyId == shortyId)
            .ToListAsync();
    }

    public async Task AddAsync(ClickDetailEntity entity)
    {
        await dbContext.ClickDetails.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }
}