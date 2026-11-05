using NotificationPlatform.Domain.Entities;

namespace NotificationPlatform.Domain.Interfaces;

// DIP: upper layers depend on this abstraction instead of concrete persistence implementations.
public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Notification>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
}
