using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shorty.Dal.Entities;

namespace Shorty.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string username, string email);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid id);
    }
}
