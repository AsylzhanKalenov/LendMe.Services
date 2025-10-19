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

public class ChannelTemplate
{
    public string Subject { get; set; } // Для email
    public string HtmlBody { get; set; } // Для email
    public string PlainTextBody { get; set; } // Для SMS и fallback
    public PushNotificationData PushData { get; set; } // Для push
}

public class PushNotificationData
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string ImageUrl { get; set; }
    public string Sound { get; set; } // default, notification.mp3
    public string ChannelId { get; set; } // Android notification channel
    public int? Badge { get; set; } // iOS badge number
    public Dictionary<string, string> Data { get; set; } // Custom data
}