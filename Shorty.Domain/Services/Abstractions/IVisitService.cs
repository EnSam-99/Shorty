namespace Shorty.Domain.Services.Abstractions;

public interface IVisitService
{
    public Task AddVisitAsync(string shortyUrl);
}
