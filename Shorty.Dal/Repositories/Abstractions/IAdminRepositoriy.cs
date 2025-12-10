namespace Shorty.Dal.Repositories.Abstractions;

public interface IAdminRepositoriy
{
    public Task<List<UsersAdminDto>> GetAllUsersForAdminAsync();
    public Task DeleteShortyByIdAsync(int id);  
    public Task<int> DeleteAllInactiveLinks();
    public Task<int> DeactivateExpiredShortiesAsync();
    public Task DeleteUserAsync(int id);
}
