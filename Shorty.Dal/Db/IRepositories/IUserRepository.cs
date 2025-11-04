using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Db.IRepositories;

public interface IUserRepository<T>
{
    public Task<IEnumerable<T>> GetAllUsersAsync();
    public Task AddUserAsync(T user);   
    public Task<bool> ExistsByUser(string userName, string email);
}
