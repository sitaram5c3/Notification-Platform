using Microsoft.Extensions.Logging;
using NotificationPlatform.Domain.Entities;
using NotificationPlatform.Domain.Enums;
using NotificationPlatform.Domain.Interfaces;

namespace NotificationPlatform.Infrastructure.Providers;

// Strategy Pattern + LSP.
public class SmsProvider(ILogger<SmsProvider> logger) : INotificationProvider
{
    public NotificationType ChannelType => NotificationType.Sms;

    public Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Mock SMS sent. To: {Recipient}, Id: {NotificationId}, MessageLength: {MessageLength}",
            notification.Recipient,
            notification.NotificationId,
            notification.Message.Length);

        return Task.CompletedTask;
    }
}
