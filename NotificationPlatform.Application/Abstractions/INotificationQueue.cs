using NotificationPlatform.Domain.Entities;

namespace NotificationPlatform.Application.Abstractions;

public interface INotificationQueue
{
    Task EnqueueAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<Notification?> DequeueAsync(CancellationToken cancellationToken = default);
}
