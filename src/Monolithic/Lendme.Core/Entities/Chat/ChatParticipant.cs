namespace Lendme.Core.Entities.Chat;

public class ChatParticipant
{
    public Guid UserId { get; set; }
    public string DisplayName { get; set; }
    public string AvatarUrl { get; set; }
    public ParticipantRole Role { get; set; } // Owner, Renter, Support, Admin
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
    public DateTime? LastSeenAt { get; set; }
    public DateTime? LastTypingAt { get; set; }
    public bool IsTyping { get; set; }
    public int UnreadCount { get; set; }
    public bool IsMuted { get; set; }
    public DateTime? MutedUntil { get; set; }
    public ParticipantStatus Status { get; set; } // Active, Left, Blocked
    public ChatNotificationSettings NotificationSettings  { get; set; }
}

public class ChatNotificationSettings
{
    public bool AllMessages { get; set; }
    public bool MentionsOnly { get; set; }
    public bool DirectMessages { get; set; }
    public bool BookingUpdates { get; set; }
    public bool SystemMessages { get; set; }
    public Dictionary<string, bool> CustomSettings { get; set; }
}

public enum ParticipantRole
{
    Owner, Renter
}

public enum ParticipantStatus
{
    Active, Left, Blocked, Invited
}