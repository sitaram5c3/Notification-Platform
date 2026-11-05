using NotificationPlatform.Domain.Enums;
using NotificationPlatform.Domain.Interfaces;

namespace NotificationPlatform.Application.Factories;

// OCP + DIP: consumers request providers through this abstraction, enabling new channels without changing consumers.
public interface INotificationProviderFactory
{
    INotificationProvider GetProvider(NotificationType notificationType);
}
