using Microsoft.Extensions.Logging;
using NotificationPlatform.Domain.Entities;
using NotificationPlatform.Domain.Enums;
using NotificationPlatform.Domain.Interfaces;

namespace NotificationPlatform.Application.Services;

// SRP: orchestrates notification lifecycle only.
// DIP: depends on abstractions (repository and provider interface).
// ISP: consumes focused contracts (INotificationRepository, INotificationProvider) instead of fat interfaces.
public class NotificationService(
    INotificationRepository repository,
    IEnumerable<INotificationProvider> providers,
    ILogger<NotificationService> logger) : INotificationService
{
    public async Task<Notification> SubmitAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        notification.Status = NotificationStatus.Pending;
        await repository.AddAsync(notification, cancellationToken);

        try
        {
            // OCP: new channels can be added by implementing INotificationProvider without changing service flow.
            // LSP: any valid INotificationProvider implementation is substitutable here.
            var provider = providers.FirstOrDefault(p => p.ChannelType == notification.NotificationType)
                ?? throw new NotSupportedException($"Notification channel '{notification.NotificationType}' is not registered.");

            await provider.SendAsync(notification, cancellationToken);
            notification.Status = NotificationStatus.Sent;
        }
        catch (Exception ex)
        {
            notification.Status = NotificationStatus.Failed;
            logger.LogError(ex, "Notification {NotificationId} failed for {Channel}", notification.NotificationId, notification.NotificationType);
            throw;
        }
        finally
        {
            await repository.UpdateAsync(notification, cancellationToken);
        }

        logger.LogInformation("Notification {NotificationId} processed with status {Status}",
            notification.NotificationId,
            notification.Status);

        return notification;
    }

    public async Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        return await repository.GetByIdAsync(notificationId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Notification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await repository.GetAllAsync(cancellationToken);
    }
}
