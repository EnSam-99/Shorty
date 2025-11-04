using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Request;
using Shorty.Domain.Models.Response;
using Shorty.Domain.Utilities;

namespace Shorty.Domain.Services;

public class ShortyUrlService : IShortyUrlService
{
	private readonly AppDbContext _dbContext;

	public ShortyUrlService(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<ShortyCreateDto> CreateShortyAsync(ShortyCreateRequestModel model)
	{
		try
		{
			var user = await _dbContext.Users
				.FirstOrDefaultAsync(u => u.Email == model.Email);

			if (user == null)
			{
				user = new User { Email = model.Email };
				await _dbContext.Users.AddAsync(user);
				await _dbContext.SaveChangesAsync();
			}

			var nextVal = await _dbContext.Database
				.SqlQueryRaw<long>("SELECT nextval('short_code_seq') AS \"Value\"")
				.FirstAsync();

			var shortCode = Base62.Encode((ulong)nextVal);

			var newShorty = new ShortLink
			{
				Url = model.Url,
				ShortyUrl = shortCode,
				CreatedDate = DateTime.UtcNow,
				UserId = user.Id
			};

			await _dbContext.Shorties.AddAsync(newShorty);
			await _dbContext.SaveChangesAsync();

			return new ShortyCreateDto
			{
				Id = newShorty.Id,
				ShortyUrl = newShorty.ShortyUrl
			};
		}
		catch (Exception)
		{
			throw;
		}
	}

	public async Task<List<ShortyListDto>> GetShortiesByEmailAsync(string email)
	{
		try
		{
			var user = await _dbContext.Users
				.Include(u => u.Shorties)
				.FirstOrDefaultAsync(u => u.Email == email);

			if (user == null || user.Shorties == null)
			{
				return new List<ShortyListDto>();
			}

			return user.Shorties.Select(s => new ShortyListDto
			{
				Id = s.Id,
				OriginalUrl = s.Url,
				ShortyUrl = s.ShortyUrl,
				CreatedDate = s.CreatedDate
			}).ToList();
		}
		catch (Exception)
		{
			throw;
		}
	}
	public async Task<ShortyStatsDto?> GetStatsByCodeAsync(string shortCode)
	{
		try
		{
			var s = await _dbContext.Shorties
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.ShortyUrl == shortCode);

			if (s is null) return null;

			return new ShortyStatsDto
			{
				ShortId = s.Id,
				Clicks = s.Clicks,
				LastAccessed = s.LastAccessed
			};
		}
		catch (Exception)
		{
			throw;
		}
	}
	public async Task<string?> ResolveAsync(string shortCode)
	{
		try
		{
			var s = await _dbContext.Shorties.AsNoTracking()
						 .FirstOrDefaultAsync(x => x.ShortyUrl == shortCode);
			return s?.Url;
		}
		catch (Exception)
		{
			throw;
		}
	}
	public async Task<string?> ResolveAndTrackAsync(string shortCode)
	{
		try
		{
			var s = await _dbContext.Shorties.FirstOrDefaultAsync(x => x.ShortyUrl == shortCode);
			if (s is null) return null;

			s.Clicks += 1;
			s.LastAccessed = DateTime.UtcNow;
			await _dbContext.SaveChangesAsync();

			return s.Url;
		}
		catch (Exception)
		{
			throw;
		}
	}
	public async Task<List<(string ShortCode, int Clicks)>> GetTopShortiesAsync(int take = 10)
	{
		try
		{
			take = Math.Clamp(take, 1, 100);
			return await _dbContext.Shorties
				.AsNoTracking()
				.OrderByDescending(x => x.Clicks)
				.Take(take)
				.Select(x => new ValueTuple<string, int>(x.ShortyUrl, x.Clicks))
				.ToListAsync();
		}
		catch (Exception)
		{
			throw;
		}
	}
}
