using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NotificationPlatform.Domain.Interfaces;
using NotificationPlatform.Infrastructure.Persistence;
using NotificationPlatform.Infrastructure.Providers;

namespace NotificationPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.TryAddSingleton<INotificationRepository, InMemoryNotificationRepository>();

        // OCP + LSP: add a new provider implementation without changing service logic.
        services.AddScoped<INotificationProvider, EmailProvider>();
        services.AddScoped<INotificationProvider, SmsProvider>();
        services.AddScoped<INotificationProvider, PushProvider>();

        return services;
    }
}
