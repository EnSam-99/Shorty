using Shorty.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Services.Abstractions
{
	public  interface IShortyExpiryService
	{
		Task CheckAndExpireShortiesAsync();
		Task<IEnumerable<ShortyEntity>> GetExpiredShortiesAsync();
	}
}
