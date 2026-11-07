using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NotificationPlatform.Application.Commands;
using NotificationPlatform.Application.Handlers;
using NotificationPlatform.Application.Services;
using NotificationPlatform.Application.Validation;

namespace NotificationPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ISendNotificationHandler, SendNotificationHandler>();
        services.AddScoped<IValidator<SendNotificationCommand>, SendNotificationCommandValidator>();

        return services;
    }
}
