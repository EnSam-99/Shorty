using Shorty.Dal.Models;
using Shorty.Models;

namespace Shorty.Services.IServices;

public interface IUserService<T>
{
  public Task<T> CreateUserAsync(string userName,string email);
    public Task<T> UpdateUserAsync(UserDto dto, Guid id);
    public Task<T> DeleteUserAsync(Guid id);
    public Task<T> GetUserAsync(Guid id);

   public Task<IEnumerable<User>> GetAllAsync();
}
