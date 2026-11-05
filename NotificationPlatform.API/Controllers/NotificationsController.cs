using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NotificationPlatform.Application.Commands;
using NotificationPlatform.Application.Handlers;
using NotificationPlatform.Application.Services;

namespace NotificationPlatform.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class NotificationsController(
    IValidator<SendNotificationCommand> validator,
    ISendNotificationHandler sendNotificationHandler,
    INotificationService notificationService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).Distinct().ToArray());

            return ValidationProblem(new ValidationProblemDetails(errors));
        }

        var result = await sendNotificationHandler.HandleAsync(command, cancellationToken);

        return AcceptedAtAction(nameof(GetNotificationById), new { id = result.NotificationId }, result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotifications(CancellationToken cancellationToken)
    {
        var notifications = await notificationService.GetAllAsync(cancellationToken);
        return Ok(notifications);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotificationById(Guid id, CancellationToken cancellationToken)
    {
        var notification = await notificationService.GetByIdAsync(id, cancellationToken);
        if (notification is null)
        {
            return NotFound();
        }

        return Ok(notification);
    }
}
