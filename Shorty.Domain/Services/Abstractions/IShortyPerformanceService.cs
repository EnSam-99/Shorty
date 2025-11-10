using Shorty.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Services.Abstractions
{
    public interface IShortyPerformanceService
    {
        Task RecalculateScoresAsync();
        Task<IEnumerable<ShortyStatDto>> GetTopPerformingShortiesAsync(int limit = 10);
    }
}
