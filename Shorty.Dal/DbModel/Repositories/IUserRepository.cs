using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.DbModel.Repositories;

public interface IUserRepository<T>
{
    public Task<IEnumerable<T>> GetAllUsersAsync();
    public Task AddUserAsync(T user);
    public Task UpdateUserAsync(T user, Guid id);
    public Task DeleteUserAsync(Guid id);
    public Task<bool> ExistsByUser(string userName, string email);
}
