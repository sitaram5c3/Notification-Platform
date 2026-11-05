using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NotificationPlatform.Application.Abstractions;
using NotificationPlatform.Application.Services;

namespace NotificationPlatform.Infrastructure.BackgroundServices;

public class NotificationProcessorBackgroundService(
    INotificationQueue queue,
    IServiceScopeFactory scopeFactory,
    ILogger<NotificationProcessorBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var notification = await queue.DequeueAsync(stoppingToken);
                if (notification is null)
                {
                    continue;
                }

                using var scope = scopeFactory.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                await notificationService.ProcessAsync(notification.NotificationId, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while processing notification queue item");
            }
        }
    }
}
