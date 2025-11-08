namespace Shorty.Domain.Services.Abstractions;

public interface IUserService<T>
{
    public Task<T> CreateUserAsync(string userName, string email);
}
