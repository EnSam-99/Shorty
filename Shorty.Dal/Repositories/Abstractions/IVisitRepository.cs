using Shorty.Dal.Entities;

namespace Shorty.Dal.Repositories.Abstractions;

public interface IVisitRepository
{
    public Task AddVisitAsync(VisitEntity entity);
}
