using NotificationPlatform.Domain.Entities;
using NotificationPlatform.Domain.Enums;

namespace NotificationPlatform.Domain.Interfaces;

// ISP + LSP: provider contract is focused and allows interchangeable channel implementations.
public interface INotificationProvider
{
    NotificationType ChannelType { get; }
    Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
}
