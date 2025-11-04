using Shorty.Dal.Entities;

namespace Shorty.Dal.Repository.Interface;

public interface IDeletionRepository
{
    Task<ShortyLink?> GetByIdAsync(int id);
    Task<List<ShortyLink?>> GetByEmailAsync(string email);
    
    Task UpdateAsync(ShortyLink entity);
    
    
}