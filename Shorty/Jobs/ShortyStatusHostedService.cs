using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class ShortyStatusHostedService(IServiceScopeFactory scopeFactory,ILogger<ShortyStatusHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("ShortyStatusHostedService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ShortyStatusService>();

            await service.UpdateShortiesStatusAsync();

            logger.LogInformation("ShortyStatusHostedService completed run at {time}", DateTime.UtcNow);

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        logger.LogInformation("ShortyStatusHostedService stopped.");
    }
}

