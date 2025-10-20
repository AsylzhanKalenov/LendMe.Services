namespace LendMe.Shared.Core.Entities.NotificationService;

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
// Enums
public enum NotificationType 
{ 
    // Booking related
    BookingRequest, 
    BookingConfirmed, 
    BookingCancelled,
    BookingReminder,
    
    // Payment related
    PaymentReceived, 
    PaymentFailed,
    PayoutCompleted,
    
    // Messages
    MessageReceived,
    
    // Reviews
    ReviewReceived,
    ReviewReminder,
    
    // Items
    ItemAvailable, 
    PriceChanged,
    ItemExpiring,
    
    // Verification
    VerificationRequired, 
    VerificationApproved,
    
    // Security
    SecurityAlert,
    LoginFromNewDevice,
    
    // System
    SystemMaintenance,
    AppUpdate,
    PolicyUpdate
}
public enum NotificationChannel { Push, Email, SMS, InApp }
public enum NotificationPriority { Low, Normal, High, Critical }
public enum DevicePlatform { iOS, Android }
public enum NotificationStatus { Created, Scheduled, Sent, Delivered, Failed, Expired }