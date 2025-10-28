using Shorty.Dal.Models;
using Shorty.Models;

namespace Shorty.Services.IServices;

public interface IUserService<T>
{
  public  Task<T> CreateUserAsync(UserDto dto);
    public Task<T> UpdateUserAsync(UserDto dto, Guid id);
    public Task<T> DeleteUserAsync(Guid id);
    public Task<T> GetUserAsync(Guid id);

   public Task<IEnumerable<User>> GetAllAsync();
}
