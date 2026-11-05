using NotificationPlatform.Application.Commands;
using NotificationPlatform.Application.Services;
using NotificationPlatform.Domain.Entities;

namespace NotificationPlatform.Application.Handlers;

public interface ISendNotificationHandler
{
    Task<Notification> HandleAsync(SendNotificationCommand command, CancellationToken cancellationToken = default);
}

// SRP: command handler maps command to domain entity and delegates execution to service.
public class SendNotificationHandler(INotificationService notificationService) : ISendNotificationHandler
{
    public async Task<Notification> HandleAsync(SendNotificationCommand command, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            NotificationId = command.NotificationId == Guid.Empty ? Guid.NewGuid() : command.NotificationId,
            Recipient = command.Recipient,
            Subject = command.Subject,
            Message = command.Message,
            NotificationType = command.NotificationType,
            Priority = command.Priority,
            CreatedDate = command.CreatedDate == default ? DateTime.UtcNow : command.CreatedDate
        };

        return await notificationService.SubmitAsync(notification, cancellationToken);
    }
}
