namespace Shorty.Domain.Services.Abstractions;

public interface IVisitService
{
    Task AddVisitAsync(string shortyUrl);
}
