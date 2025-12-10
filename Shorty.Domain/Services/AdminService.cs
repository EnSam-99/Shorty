using Shorty.Dal;
using Shorty.Dal.Repositories.Abstractions;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services;

public class AdminService(IAdminRepositoriy _repository) : IAdminService
{
    public Task<int> DeactivateExpiredShortiesAsync()
    {
        var deactivate = _repository.DeactivateExpiredShortiesAsync();
        return deactivate;
    }

    public Task<int> DeleteAllInactiveLinks()
    {
        var delete = _repository.DeleteAllInactiveLinks();
        return delete;
    }

    public async Task DeleteShortyByIdAsync(int id)
    {
        await _repository.DeleteShortyByIdAsync(id);
    }

    public Task<List<UsersAdminDto>> GetAllUsersForAdminAsync()
    {
        var users = _repository.GetAllUsersForAdminAsync();
        return users;
    }

    public async Task DeleteUserAsync(int id) {       
      await _repository.DeleteUserAsync(id);
    }
}
