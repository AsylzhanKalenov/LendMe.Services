using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Lendme.Core.Entities.Chat;

namespace Lendme.Infrastructure.MongoPersistence.Documents.ChatDocs;

public class MessageDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    [BsonElement("chatId")]
    [BsonRepresentation(BsonType.String)]
    public Guid ChatId { get; set; }
    
    [BsonElement("senderId")]
    [BsonRepresentation(BsonType.String)]
    public Guid SenderId { get; set; }
    
    [BsonElement("type")]
    public MessageType Type { get; set; }
    
    [BsonElement("status")]
    public MessageStatus Status { get; set; }
    
    [BsonElement("content")]
    public MessageContentDocument Content { get; set; }
    
    [BsonElement("sentAt")]
    public DateTime SentAt { get; set; }
    
    [BsonElement("editedAt")]
    public DateTime? EditedAt { get; set; }
    
    [BsonElement("deletedAt")]
    public DateTime? DeletedAt { get; set; }
    
    [BsonElement("replyToMessageId")]
    [BsonRepresentation(BsonType.String)]
    public Guid? ReplyToMessageId { get; set; }
    
    [BsonElement("readReceipts")]
    public List<MessageReadReceiptDocument> ReadReceipts { get; set; } = new();
}

public class MessageContentDocument
{
    [BsonElement("text")]
    public string Text { get; set; }
    
    [BsonElement("attachment")]
    public MessageAttachmentDocument Attachment { get; set; }
    
    [BsonElement("location")]
    public LocationShareDocument Location { get; set; }
    
    [BsonElement("voiceMessage")]
    public VoiceMessageDocument VoiceMessage { get; set; }
}

public class MessageAttachmentDocument
{
    [BsonElement("type")]
    public AttachmentType Type { get; set; }
    
    [BsonElement("fileUrl")]
    public string FileUrl { get; set; }
    
    [BsonElement("thumbnailUrl")]
    public string ThumbnailUrl { get; set; }
    
    [BsonElement("fileName")]
    public string FileName { get; set; }
    
    [BsonElement("mimeType")]
    public string MimeType { get; set; }
    
    [BsonElement("fileSize")]
    public long FileSize { get; set; }
    
    [BsonElement("width")]
    public int? Width { get; set; }
    
    [BsonElement("height")]
    public int? Height { get; set; }
    
    [BsonElement("duration")]
    public int? Duration { get; set; }
    
    [BsonElement("metadata")]
    public Dictionary<string, string> Metadata { get; set; } = new();
}

public class LocationShareDocument
{
    [BsonElement("latitude")]
    public double Latitude { get; set; }
    
    [BsonElement("longitude")]
    public double Longitude { get; set; }
    
    [BsonElement("address")]
    public string Address { get; set; }
    
    [BsonElement("placeName")]
    public string PlaceName { get; set; }
    
    [BsonElement("mapPreviewUrl")]
    public string MapPreviewUrl { get; set; }
}

public class VoiceMessageDocument
{
    [BsonElement("audioUrl")]
    public string AudioUrl { get; set; }
    
    [BsonElement("duration")]
    public int Duration { get; set; }
    
    [BsonElement("waveform")]
    public string Waveform { get; set; }
    
    [BsonElement("transcript")]
    public string Transcript { get; set; }
}

public class MessageReadReceiptDocument
{
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    
    [BsonElement("readAt")]
    public DateTime ReadAt { get; set; }
}