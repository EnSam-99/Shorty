using Shorty.Domain.Services.Abstractions;

namespace Shorty.Services
{
	public class ShortyExpiryBackgroundService : BackgroundService
	{
		private const int expireHour = 1;

		private readonly IShortyExpiryService shortyExpiryService;
		
		public ShortyExpiryBackgroundService(IShortyExpiryService shortyExpiryService)
		{
			this.shortyExpiryService = shortyExpiryService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await shortyExpiryService.CheckAndExpireShortiesAsync();

					await Task.Delay(TimeSpan.FromHours(expireHour), stoppingToken);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error in ShortyExpiryService: {ex.Message}");
				}
			}
		}
	}
}
