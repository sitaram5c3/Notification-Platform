using FluentValidation;
using NotificationPlatform.Application.Commands;
using NotificationPlatform.Domain.Enums;

namespace NotificationPlatform.Application.Validation;

public class SendNotificationCommandValidator : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationCommandValidator()
    {
        RuleFor(x => x.Recipient)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Subject)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(4000);

        RuleFor(x => x.Priority)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.NotificationType)
            .Must(type => Enum.IsDefined(typeof(NotificationType), type))
            .WithMessage("Unsupported notification type.");
    }
}
