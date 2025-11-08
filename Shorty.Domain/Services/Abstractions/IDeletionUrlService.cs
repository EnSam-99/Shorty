namespace Shorty.Domain.Services.Abstractions;

public interface IDeletionUrlService
{
    Task<bool> DeleteByIdAsync(int id);
    Task<bool> DeleteByEmailAsync(string email);
    Task<bool> DeactivateByIdAsync(int id);
}