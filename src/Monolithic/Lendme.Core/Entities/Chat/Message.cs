using Lendme.Core.Entities.Profile;

namespace Lendme.Core.Entities.Chat;

public class Message
{
    // ObjectId 
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public MessageType Type { get; set; }
    public MessageStatus Status { get; set; }
    
    // Контент
    public MessageContent Content { get; set; }
    
    // Временные метки
    public DateTime SentAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    // Доставка и прочтение
    // public List<MessageDeliveryStatus> DeliveryStatus { get; set; }
    // public List<MessageReadReceipt> ReadReceipts { get; set; }
    
    // Ответы и реакции
    public Guid? ReplyToMessageId { get; set; }
    //public List<MessageReaction> Reactions { get; set; }
    
    // Метаданные
    //public MessageMetadata Metadata { get; set; }
}

public class MessageContent
{
    // Текстовое сообщение
    public string Text { get; set; }
    // public List<MessageMention> Mentions { get; set; }
    // public List<MessageLink> Links { get; set; }
    
    // Вложения
    public MessageAttachment Attachment { get; set; }
    
    // Специальные типы
    public LocationShare Location { get; set; }
    // public BookingOffer BookingOffer { get; set; }
    // public SystemMessage SystemMessage { get; set; }
    public VoiceMessage VoiceMessage { get; set; }
}

public class MessageAttachment
{
    public AttachmentType Type { get; set; } // Image, Video, File, Audio
    public string FileUrl { get; set; }
    public string ThumbnailUrl { get; set; }
    public string FileName { get; set; }
    public string MimeType { get; set; }
    public long FileSize { get; set; }
    public int? Width { get; set; } // Для изображений/видео
    public int? Height { get; set; }
    public int? Duration { get; set; } // Для видео/аудио в секундах
    public Dictionary<string, string> Metadata { get; set; }
}

public class VoiceMessage
{
    public string AudioUrl { get; set; }
    public int Duration { get; set; } // Секунды
    public string Waveform { get; set; } // Визуализация волны
    public string Transcript { get; set; } // Автоматическая расшифровка
}

public class LocationShare
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; }
    public string PlaceName { get; set; }
    public string MapPreviewUrl { get; set; }
}

public enum AttachmentType { Image, Video, File, Audio, Document }
public enum MessageType { Text, Image, File, Location, BookingOffer, System }
public enum MessageStatus { Sending, Sent, Delivered, Read, Failed, Deleted }