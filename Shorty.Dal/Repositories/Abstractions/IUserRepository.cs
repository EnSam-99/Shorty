namespace Shorty.Dal.Repositories.Abstractions;

public interface IUserRepository<T>
{
    public Task<IEnumerable<T>> GetAllUsersAsync();
    public Task AddUserAsync(T user);
    public Task<bool> ExistsByUser(string userName, string email);
}
