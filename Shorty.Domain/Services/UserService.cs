using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Models;
using Shorty.Models;
using Shorty.Services.IServices;

namespace Shorty.Services;

public class UserService: IUserService<UserEntity>
{
    private readonly IUserRepository<UserEntity> _userRepository;
    public UserService(IUserRepository<UserEntity> userRepository)=>  _userRepository = userRepository;   
    public async Task<UserEntity> CreateUserAsync(string userName, string email)
    {
        if (string.IsNullOrEmpty(userName))
        {
            throw new ArgumentNullException(nameof(userName));
        }
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        var exist = await _userRepository.ExistsByUser(userName, email);

        if (exist)
        {
            throw new ArgumentException(" User is Exists");
        }

        var user = new UserEntity
        {
            Name = userName,
            Email = email            
        };
        await _userRepository.AddUserAsync(user);
        return user;
    }
}
