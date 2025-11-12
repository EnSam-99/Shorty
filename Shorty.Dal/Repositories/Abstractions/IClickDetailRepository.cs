namespace Shorty.Dal.Repositories.Abstractions;
using Shorty.Dal.Entities;

public interface IClickDetailRepository
{
    Task<IEnumerable<ClickDetailEntity>> GetByShortyIdAsync(int shortyId);
    Task AddAsync(ClickDetailEntity entity);
}