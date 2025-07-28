namespace Lendme.Core.Entities.NotificationService;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public NotificationChannel Channel { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public NotificationPriority Priority { get; set; }
    
    // Metadata
    public Dictionary<string, string> Data { get; set; }
    public string DeepLink { get; set; } // App navigation
    
    // Status
    public NotificationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public string FailureReason { get; set; }
    
    // Scheduling
    public DateTime? ScheduledFor { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

// Enums
public enum NotificationType 
{ 
    BookingRequest, BookingConfirmed, BookingCancelled,
    PaymentReceived, PaymentFailed, 
    MessageReceived, ReviewReceived,
    ItemAvailable, PriceChanged,
    VerificationRequired, SecurityAlert
}
public enum NotificationChannel { Push, Email, SMS, InApp }
public enum NotificationPriority { Low, Normal, High, Critical }
public enum DevicePlatform { iOS, Android }