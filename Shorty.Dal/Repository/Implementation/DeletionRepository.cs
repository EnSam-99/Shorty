using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;
using System.Linq;
using Shorty.Dal.Repository.Interface;


namespace Shorty.Dal.Repository.Implementation;

public class DeletionRepository : IDeletionRepository
{
    private AppDbContext _context;
    public DeletionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ShortyLink?> GetByIdAsync(int id)
    {
        return await _context.Shorties.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<ShortyLink>> GetByEmailAsync(string email)
    {
        return await _context.Shorties
            .Include(s => s.User)
            .Where(s => s.User.Email == email)
            .ToListAsync();
    }

    public async Task UpdateAsync(ShortyLink entity)
    {
        entity.IsActive = false;
        entity.DeletedAt = DateTime.UtcNow;
        _context.Shorties.Update(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(ShortyLink entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<ShortyLink> entities)
    {
        _context.Shorties.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }
    
}