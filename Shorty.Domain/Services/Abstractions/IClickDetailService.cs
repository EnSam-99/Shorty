using Shorty.Domain.Models;

namespace Shorty.Domain.Services.Abstractions;

public interface IClickDetailService
{
    Task<ClickDetailDto> GetClickSummaryAsync(int shortyId);
    Task  AddClickAsync(int shortyId, string ipAddress);
}