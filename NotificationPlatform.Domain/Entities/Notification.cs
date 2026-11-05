using NotificationPlatform.Domain.Enums;

namespace NotificationPlatform.Domain.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType NotificationType { get; set; }
    public string Priority { get; set; } = "Normal";
    public DateTime CreatedDate { get; set; }
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
}
