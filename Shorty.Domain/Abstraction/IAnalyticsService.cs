using Shorty.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Abstraction
{
    public interface IAnalyticsService
    {
        Task<List<TopShortLinkDto>> GetTopShortLinksAsync(string email);
    }
}
