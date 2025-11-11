namespace Shorty.Dal.Repositories.Abstractions;

public interface IAdminRepositoriy
{
    public Task<List<AdminUsersDto>> GetAllUsersForAdminAsync();
    public Task DeleteShortyByIdAsync(int id);
}
