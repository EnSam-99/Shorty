using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shorty.Dal;
using Shorty.Dal.Entities;
using Shorty.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Services
{
	public class ShortyExpiryService : IShortyExpiryService
	{
		private readonly IServiceProvider _serviceProvider;

		public ShortyExpiryService(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public async Task CheckAndExpireShortiesAsync()
		{
			using var scope = _serviceProvider.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var expired = await db.Shorties
				.Where(s => s.IsActive && s.ExpiredAt < DateTime.UtcNow)
				.ToListAsync();

			if (expired.Any())
			{
				foreach (var s in expired)
					s.IsActive = false;

				await db.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<ShortyEntity>> GetExpiredShortiesAsync()
		{
			using var scope = _serviceProvider.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			return await db.Shorties
				.AsNoTracking()
				.Where(s => !s.IsActive && s.ExpiredAt < DateTime.UtcNow)
				.OrderByDescending(s => s.ExpiredAt)
				.ToListAsync();
		}
	}
}
