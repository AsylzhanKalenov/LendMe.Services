namespace Lendme.Core.Entities.NotificationService;

public class UserNotificationPreferences
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    // Channel preferences
    public bool EmailEnabled { get; set; }
    public bool PushEnabled { get; set; }
    public bool SmsEnabled { get; set; }
    
    // Type preferences
    //public List<NotificationTypePreference> TypePreferences { get; set; }
    
    // Quiet hours
    public TimeSpan? QuietHoursStart { get; set; }
    public TimeSpan? QuietHoursEnd { get; set; }
    public string TimeZone { get; set; }
}

public class NotificationTypePreference
{
    public Guid Id { get; set; }
    public NotificationType Type { get; set; }
    public bool EmailEnabled { get; set; }
    public bool PushEnabled { get; set; }
    public bool SmsEnabled { get; set; }
    public bool InAppEnabled { get; set; }
    public NotificationFrequency Frequency { get; set; } // Immediate, Daily, Weekly
}

public enum NotificationFrequency { Immediate, Hourly, Daily, Weekly, Never }