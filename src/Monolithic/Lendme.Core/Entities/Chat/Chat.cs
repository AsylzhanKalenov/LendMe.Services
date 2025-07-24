namespace Lendme.Core.Entities.Chat;

public class Chat
{
    // ObjectId
    public Guid Id { get; set; }
    public ChatType Type { get; set; } // Direct, Booking, Support, Group
    public Guid? ItemId { get; set; } // Для чатов по товару
    public Guid? BookingId { get; set; } // Для чатов по бронированию
    
    // Участники
    public List<ChatParticipant> Participants { get; set; }
    
    // Метаданные чата
    //public ChatMetadata Metadata { get; set; }
    
    // Временные метки
    public DateTime CreatedAt { get; set; }
    public DateTime LastMessageAt { get; set; }
    public DateTime? ArchivedAt { get; set; }
    
    // Статус
    public ChatStatus Status { get; set; }
    public ChatSettings Settings { get; set; }
    
    // Счетчики
    public int MessageCount { get; set; }
    public int UnreadCount { get; set; }
}

public class ChatSettings
{
    public bool IsReadOnly { get; set; }
    public bool AllowFiles { get; set; }
    public bool AllowVoiceMessages { get; set; }
    public bool AllowLocation { get; set; }
    public bool ShowTypingIndicator { get; set; }
    public bool AutoArchiveEnabled { get; set; }
    public int AutoArchiveDays { get; set; } = 30;
    public int MaxFileSize { get; set; } = 10485760; // 10MB
    public List<string> AllowedFileTypes { get; set; }
}

public enum ChatStatus { Active, Archived, Blocked }
public enum ChatType { Direct, Booking, Support, Group, Announcement }