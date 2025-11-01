using Shorty.Dal.DbModel.Repositories;
using Shorty.Dal.Models;
using Shorty.Models;
using Shorty.Services.IServices;

namespace Shorty.Services;

public class UserService: IUserService<User>
{
    private readonly IUserRepository<User> _userRepository;

    public UserService(IUserRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> CreateUserAsync(string userName, string email)
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
            Console.WriteLine("User is Exists");
        }

        var user = new User
        {
            Name = userName,
            Email = email,
            Id = Guid.NewGuid()
        };
        await _userRepository.AddUserAsync(user);
        return user;
    }

}
