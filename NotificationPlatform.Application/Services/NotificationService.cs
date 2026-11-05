using Microsoft.Extensions.Logging;
using NotificationPlatform.Application.Abstractions;
using NotificationPlatform.Application.Factories;
using NotificationPlatform.Domain.Entities;
using NotificationPlatform.Domain.Enums;
using NotificationPlatform.Domain.Interfaces;

namespace NotificationPlatform.Application.Services;

// SRP: orchestrates notification lifecycle only.
// DIP: depends on abstractions (repository, factory, queue, cache).
public class NotificationService(
    INotificationRepository repository,
    INotificationProviderFactory providerFactory,
    INotificationQueue queue,
    ICacheService cache,
    ILogger<NotificationService> logger) : INotificationService
{
    private const string NotificationCachePrefix = "notification:";
    private const string NotificationAllCacheKey = "notification:all";

    public async Task<Notification> SubmitAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        notification.Status = NotificationStatus.Queued;
        await repository.AddAsync(notification, cancellationToken);
        await queue.EnqueueAsync(notification, cancellationToken);

        cache.Remove(NotificationAllCacheKey);
        cache.Set(GetByIdCacheKey(notification.NotificationId), notification, TimeSpan.FromMinutes(5));

        logger.LogInformation("Notification {NotificationId} queued for {Recipient} through {Channel}",
            notification.NotificationId,
            notification.Recipient,
            notification.NotificationType);

        return notification;
    }

    public async Task ProcessAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await repository.GetByIdAsync(notificationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Notification {notificationId} not found");

        try
        {
            var provider = providerFactory.GetProvider(notification.NotificationType);
            await provider.SendAsync(notification, cancellationToken);
            notification.Status = NotificationStatus.Sent;
        }
        catch (Exception ex)
        {
            notification.Status = NotificationStatus.Failed;
            logger.LogError(ex, "Notification {NotificationId} failed for {Channel}", notificationId, notification.NotificationType);
            throw;
        }
        finally
        {
            await repository.UpdateAsync(notification, cancellationToken);
            cache.Set(GetByIdCacheKey(notification.NotificationId), notification, TimeSpan.FromMinutes(5));
            cache.Remove(NotificationAllCacheKey);
        }
    }

    public async Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        if (cache.TryGetValue(GetByIdCacheKey(notificationId), out Notification? cached) && cached is not null)
        {
            return cached;
        }

        var notification = await repository.GetByIdAsync(notificationId, cancellationToken);
        if (notification is not null)
        {
            cache.Set(GetByIdCacheKey(notificationId), notification, TimeSpan.FromMinutes(5));
        }

        return notification;
    }

    public async Task<IReadOnlyCollection<Notification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (cache.TryGetValue(NotificationAllCacheKey, out IReadOnlyCollection<Notification>? cached) && cached is not null)
        {
            return cached;
        }

        var notifications = await repository.GetAllAsync(cancellationToken);
        cache.Set(NotificationAllCacheKey, notifications, TimeSpan.FromMinutes(2));
        return notifications;
    }

    private static string GetByIdCacheKey(Guid notificationId) => $"{NotificationCachePrefix}{notificationId}";
}
