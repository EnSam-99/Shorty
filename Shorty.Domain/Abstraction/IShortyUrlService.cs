using Shorty.Domain.Models.Request;
using Shorty.Domain.Models.Response;

namespace Shorty.Domain.Abstraction;

public interface IShortyUrlService
{
    Task<ShortyCreateDto> CreateShortyAsync(ShortyCreateRequestModel model);
}
