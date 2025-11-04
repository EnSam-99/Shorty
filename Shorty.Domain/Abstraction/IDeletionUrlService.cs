using Shorty.Dal.Entities;

namespace Shorty.Domain.Abstraction;

public interface IDeletionUrlService
{
    Task<bool> DeleteByIdAsync(int id);
    Task<bool> DeleteByEmailAsync(string email);
    Task<bool> DeactivateByIdAsync(int id);
    Task<List<ShortyLink?>> GetAllAsync();
    
    
}