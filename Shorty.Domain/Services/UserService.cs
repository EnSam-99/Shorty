using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Data;
using Shorty.Domain.Interfaces;
using Shorty.Dal.Entities;

namespace Shorty.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(string username, string email)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existing != null)
                return existing;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetUserByIdAsync(Guid id)
            => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
