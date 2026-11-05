namespace NotificationPlatform.Infrastructure.Messaging;

public class AzureServiceBusQueueOptions
{
    public const string SectionName = "AzureServiceBus";
    public bool Enabled { get; set; }
    public string QueueName { get; set; } = "notifications";
}
