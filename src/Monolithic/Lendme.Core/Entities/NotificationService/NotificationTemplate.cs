namespace Lendme.Core.Entities.NotificationService;

public class NotificationTemplate
{
    public Guid Id { get; set; }
    public string Code { get; set; } // BOOKING_CONFIRMED, PAYMENT_RECEIVED
    public NotificationType Type { get; set; }
    public string TitleTemplate { get; set; }
    public string BodyTemplate { get; set; }
    public List<string> RequiredVariables { get; set; }
    public bool IsActive { get; set; }
    
    // Channel-specific templates
    public Dictionary<string, ChannelTemplate> ChannelTemplates { get; set; }
}