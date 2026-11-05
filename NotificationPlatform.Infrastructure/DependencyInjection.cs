using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NotificationPlatform.Application.Abstractions;
using NotificationPlatform.Domain.Interfaces;
using NotificationPlatform.Infrastructure.BackgroundServices;
using NotificationPlatform.Infrastructure.Caching;
using NotificationPlatform.Infrastructure.Messaging;
using NotificationPlatform.Infrastructure.Persistence;
using NotificationPlatform.Infrastructure.Providers;

namespace NotificationPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.TryAddSingleton<InMemoryNotificationQueue>();
        services.TryAddSingleton<ICacheService, InMemoryCacheService>();
        services.TryAddSingleton<INotificationRepository, InMemoryNotificationRepository>();
        services.TryAddSingleton<INotificationQueue, AzureServiceBusNotificationQueue>();

        // Strategy pattern registrations (OCP: add new provider class + registration without changing orchestrators).
        services.AddScoped<INotificationProvider, EmailProvider>();
        services.AddScoped<INotificationProvider, SmsProvider>();
        services.AddScoped<INotificationProvider, PushProvider>();

        services.AddHostedService<NotificationProcessorBackgroundService>();

        return services;
    }
}
