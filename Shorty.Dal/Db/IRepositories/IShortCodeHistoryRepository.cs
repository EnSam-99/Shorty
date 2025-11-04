using Shorty.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Db.IRepositories
{
    public interface IShortCodeHistoryRepository<T>
    {
        public Task AddHistoryAsync(T history);
        public Task<bool> ExistsOldShortCodeAsync(string oldShortCode);
        public Task<bool> ExistsNewShortCodeAsync(string newShortCode);
        public Task<IEnumerable<T>> GetAllHistoryAsync(); 
    }
}
