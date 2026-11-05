using Microsoft.Extensions.Logging;
using NotificationPlatform.Domain.Entities;
using NotificationPlatform.Domain.Enums;
using NotificationPlatform.Domain.Interfaces;

namespace NotificationPlatform.Infrastructure.Providers;

// Strategy Pattern + LSP.
public class PushProvider(ILogger<PushProvider> logger) : INotificationProvider
{
    public NotificationType ChannelType => NotificationType.Push;

    public Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Mock Push notification sent. Recipient: {Recipient}, Subject: {Subject}, Id: {NotificationId}",
            notification.Recipient,
            notification.Subject,
            notification.NotificationId);

        return Task.CompletedTask;
    }
}
