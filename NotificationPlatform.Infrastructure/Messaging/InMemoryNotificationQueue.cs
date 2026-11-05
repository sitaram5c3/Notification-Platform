using System.Threading.Channels;
using NotificationPlatform.Application.Abstractions;
using NotificationPlatform.Domain.Entities;

namespace NotificationPlatform.Infrastructure.Messaging;

public class InMemoryNotificationQueue : INotificationQueue
{
    private readonly Channel<Notification> _channel = Channel.CreateUnbounded<Notification>();

    public Task EnqueueAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(notification, cancellationToken).AsTask();
    }

    public async Task<Notification?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        var notification = await _channel.Reader.ReadAsync(cancellationToken);
        return notification;
    }
}
