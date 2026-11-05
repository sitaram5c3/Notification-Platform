using System.Collections.Concurrent;
using NotificationPlatform.Domain.Entities;
using NotificationPlatform.Domain.Interfaces;

namespace NotificationPlatform.Infrastructure.Persistence;

// Repository Pattern + SRP: persistence concerns isolated from domain/application workflows.
public class InMemoryNotificationRepository : INotificationRepository
{
    private readonly ConcurrentDictionary<Guid, Notification> _storage = new();

    public Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _storage[notification.NotificationId] = notification;
        return Task.CompletedTask;
    }

    public Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        _storage.TryGetValue(notificationId, out var notification);
        return Task.FromResult(notification);
    }

    public Task<IReadOnlyCollection<Notification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Notification> notifications = _storage.Values
            .OrderByDescending(x => x.CreatedDate)
            .ToArray();

        return Task.FromResult(notifications);
    }

    public Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _storage[notification.NotificationId] = notification;
        return Task.CompletedTask;
    }
}
