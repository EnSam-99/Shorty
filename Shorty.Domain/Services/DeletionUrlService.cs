using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Dal.Entities;
using Shorty.Dal.Repository.Interface;
using Shorty.Domain.Abstraction;

namespace Shorty.Domain.Services;

public class DeletionUrlService : IDeletionUrlService
{
    private readonly IDeletionRepository _repository;
    private readonly AppDbContext _context;
    public DeletionUrlService(IDeletionRepository repository,AppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var url = await _repository.GetByIdAsync(id);
        if (url == null) return false;
        url.IsActive = false;
        url.DeletedAt = DateTime.Now;
        
        _context.Shorties.Remove(url);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteByEmailAsync(string email)
    {
        var urls = await _repository.GetByEmailAsync(email);
        if(!urls.Any())  return false;

        foreach (var url in urls )
        {
            url.IsActive = false;
            url.DeletedAt = DateTime.Now;
        }

        _context.Shorties.RemoveRange(urls);  
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateByIdAsync(int id)
    {
        var url = await _repository.GetByIdAsync(id);
        if (url == null || !url.IsActive) return false;
        url.IsActive = false;
        await _repository.UpdateAsync(url);
        return true;
    }
    
    public async Task<List<ShortyLink>> GetAllAsync()
    {
        return await _context.Shorties
            .Include(s => s.User)
            .OrderBy(s => s.Id)
            .ToListAsync();
    }
}
