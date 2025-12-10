using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shorty.Dal;
using Shorty.Dal.Entities;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services
{
	public class ShortyExpiryService : IShortyExpiryService
	{
		private readonly IServiceProvider serviceProvider;

		public ShortyExpiryService(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public async Task CheckAndExpireShortiesAsync()
		{
			using var scope = serviceProvider.CreateScope();
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
			using var scope = serviceProvider.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			return await db.Shorties
				.AsNoTracking()
				.Where(s => !s.IsActive && s.ExpiredAt < DateTime.UtcNow)
				.OrderByDescending(s => s.ExpiredAt)
				.ToListAsync();
		}
	}
}
