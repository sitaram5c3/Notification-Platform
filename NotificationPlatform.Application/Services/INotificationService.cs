using NotificationPlatform.Domain.Entities;

namespace NotificationPlatform.Application.Services;

public interface INotificationService
{
    Task<Notification> SubmitAsync(Notification notification, CancellationToken cancellationToken = default);
    Task ProcessAsync(Guid notificationId, CancellationToken cancellationToken = default);
    Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Notification>> GetAllAsync(CancellationToken cancellationToken = default);
}
