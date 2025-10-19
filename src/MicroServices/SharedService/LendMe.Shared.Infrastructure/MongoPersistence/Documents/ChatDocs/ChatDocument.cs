using LendMe.Shared.Core.Entities.ChatService;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LendMe.Shared.Infrastructure.MongoPersistence.Documents.ChatDocs;

public class ChatDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    [BsonElement("type")]
    public ChatType Type { get; set; }
    
    [BsonElement("itemId")]
    [BsonRepresentation(BsonType.String)]
    public Guid? ItemId { get; set; }
    
    [BsonElement("bookingId")]
    [BsonRepresentation(BsonType.String)]
    public Guid? BookingId { get; set; }
    
    [BsonElement("participants")]
    public List<ChatParticipantDocument> Participants { get; set; } = new();
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("lastMessageAt")]
    public DateTime LastMessageAt { get; set; }
    
    [BsonElement("archivedAt")]
    public DateTime? ArchivedAt { get; set; }
    
    [BsonElement("status")]
    public ChatStatus Status { get; set; }
    
    [BsonElement("settings")]
    public ChatSettingsDocument Settings { get; set; }
    
    [BsonElement("messageCount")]
    public int MessageCount { get; set; }
    
    [BsonElement("unreadCount")]
    public int UnreadCount { get; set; }
}

public class ChatParticipantDocument
{
    [BsonElement("id")]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    
    [BsonElement("role")]
    public ParticipantRole Role { get; set; }
    
    [BsonElement("joinedAt")]
    public DateTime JoinedAt { get; set; }
    
    [BsonElement("leftAt")]
    public DateTime? LeftAt { get; set; }
    
    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;
    
    [BsonElement("isMuted")]
    public bool IsMuted { get; set; }
    
    [BsonElement("mutedUntil")]
    public DateTime? MutedUntil { get; set; }
    
    [BsonElement("lastReadMessageAt")]
    public DateTime? LastReadMessageAt { get; set; }
    
    [BsonElement("lastReadMessageId")]
    [BsonRepresentation(BsonType.String)]
    public Guid? LastReadMessageId { get; set; }
}

public class ChatSettingsDocument
{
    [BsonElement("isReadOnly")]
    public bool IsReadOnly { get; set; }
    
    [BsonElement("allowFiles")]
    public bool AllowFiles { get; set; }
    
    [BsonElement("allowVoiceMessages")]
    public bool AllowVoiceMessages { get; set; }
    
    [BsonElement("allowLocation")]
    public bool AllowLocation { get; set; }
    
    [BsonElement("showTypingIndicator")]
    public bool ShowTypingIndicator { get; set; }
    
    [BsonElement("autoArchiveEnabled")]
    public bool AutoArchiveEnabled { get; set; }
    
    [BsonElement("autoArchiveDays")]
    public int AutoArchiveDays { get; set; } = 30;
    
    [BsonElement("maxFileSize")]
    public int MaxFileSize { get; set; } = 10485760;
    
    [BsonElement("allowedFileTypes")]
    public List<string> AllowedFileTypes { get; set; } = new();
}

