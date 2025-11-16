using Shorty.Dal;

namespace Shorty.Domain.Services.Abstractions
{
    public interface IAdminService
    {
        public Task<List<UsersAdminDto>> GetAllUsersForAdminAsync();
        public Task DeleteShortyByIdAsync(int id);        
        public Task<int> DeleteAllInactiveLinks();
        public Task<int> DeactivateExpiredShortiesAsync();
        public Task DeleteUserAsync(int id);
    }
}
