using NotificationPlatform.Domain.Enums;
using NotificationPlatform.Domain.Interfaces;

namespace NotificationPlatform.Application.Factories;

// Factory Pattern + SRP: this class is responsible only for provider resolution.
public class NotificationProviderFactory(IEnumerable<INotificationProvider> providers) : INotificationProviderFactory
{
    private readonly Dictionary<NotificationType, INotificationProvider> _providers = providers
        .ToDictionary(p => p.ChannelType, p => p);

    public INotificationProvider GetProvider(NotificationType notificationType)
    {
        if (_providers.TryGetValue(notificationType, out var provider))
        {
            return provider;
        }

        throw new NotSupportedException($"Notification channel '{notificationType}' is not registered.");
    }
}
