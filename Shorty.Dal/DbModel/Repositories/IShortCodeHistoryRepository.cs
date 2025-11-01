using Shorty.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.DbModel.Repositories
{
    public interface IShortCodeHistoryRepository<T>
    {
        public Task AddHistoryAsync(T history);

        public Task<bool> ExsistOldShortCodeAsync(string oldShortCode);
        public Task<bool> ExsistNewShortCodeAsync(string newShortCode);
        public Task<IEnumerable<T>> GetAllHistoryAsync();
    }
}
