using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationPlatform.Application.Abstractions;
using NotificationPlatform.Domain.Entities;

namespace NotificationPlatform.Infrastructure.Messaging;

// DIP + OCP: this abstraction-driven implementation can be replaced with real Azure Service Bus integration.
public class AzureServiceBusNotificationQueue(
    InMemoryNotificationQueue fallbackQueue,
    IOptions<AzureServiceBusQueueOptions> options,
    ILogger<AzureServiceBusNotificationQueue> logger) : INotificationQueue
{
    private readonly AzureServiceBusQueueOptions _options = options.Value;

    public Task EnqueueAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Azure Service Bus abstraction enqueue called for {NotificationId}, queue {QueueName}, enabled {Enabled}",
            notification.NotificationId,
            _options.QueueName,
            _options.Enabled);

        return fallbackQueue.EnqueueAsync(notification, cancellationToken);
    }

    public Task<Notification?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return fallbackQueue.DequeueAsync(cancellationToken);
    }
}
