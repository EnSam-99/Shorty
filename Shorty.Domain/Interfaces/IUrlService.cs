using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shorty.Dal.Entities;

namespace Shorty.Domain.Interfaces
{
    public interface IUrlService
    {
        Task<UrlMapping> CreateShortUrlAsync(Guid userId, string originalUrl);
        Task<string?> GetOriginalUrlAsync(string shortCode);
        Task<IEnumerable<UrlMapping>> GetUserUrlsAsync(Guid userId);
    }
}
