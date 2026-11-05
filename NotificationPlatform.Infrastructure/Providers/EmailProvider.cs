using Microsoft.Extensions.Logging;
using NotificationPlatform.Domain.Entities;
using NotificationPlatform.Domain.Enums;
using NotificationPlatform.Domain.Interfaces;

namespace NotificationPlatform.Infrastructure.Providers;

// Strategy Pattern + LSP: concrete strategy interchangeable through INotificationProvider.
public class EmailProvider(ILogger<EmailProvider> logger) : INotificationProvider
{
    public NotificationType ChannelType => NotificationType.Email;

    public Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Mock Email sent. To: {Recipient}, Subject: {Subject}, Id: {NotificationId}",
            notification.Recipient,
            notification.Subject,
            notification.NotificationId);

        return Task.CompletedTask;
    }
}
