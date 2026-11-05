using NotificationPlatform.Domain.Enums;

namespace NotificationPlatform.Application.Commands;

public class SendNotificationCommand
{
    public Guid NotificationId { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType NotificationType { get; set; }
    public string Priority { get; set; } = "Normal";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
