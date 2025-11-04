using Shorty.Dal.Models;
using Shorty.Models;

namespace Shorty.Services.IServices;

public interface IUserService<T>
{
  public Task<T> CreateUserAsync(string userName,string email);
}
