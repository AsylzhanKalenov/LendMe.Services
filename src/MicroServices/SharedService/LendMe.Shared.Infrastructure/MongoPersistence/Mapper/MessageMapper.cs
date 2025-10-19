using LendMe.Shared.Core.Entities.ChatService;
using LendMe.Shared.Infrastructure.MongoPersistence.Documents.ChatDocs;

namespace LendMe.Shared.Infrastructure.MongoPersistence.Mapper;

public static class MessageMapper
{
    public static Message ToEntity(MessageDocument document)
    {
        if (document == null) return null;

        return new Message
        {
            Id = document.Id,
            ChatId = document.ChatId,
            SenderId = document.SenderId,
            Type = document.Type,
            Status = document.Status,
            Content = ToEntity(document.Content),
            SentAt = document.SentAt,
            EditedAt = document.EditedAt,
            DeletedAt = document.DeletedAt,
            ReplyToMessageId = document.ReplyToMessageId
        };
    }

    public static MessageDocument ToDocument(Message entity)
    {
        if (entity == null) return null;

        return new MessageDocument
        {
            Id = entity.Id,
            ChatId = entity.ChatId,
            SenderId = entity.SenderId,
            Type = entity.Type,
            Status = entity.Status,
            Content = ToDocument(entity.Content),
            SentAt = entity.SentAt,
            EditedAt = entity.EditedAt,
            DeletedAt = entity.DeletedAt,
            ReplyToMessageId = entity.ReplyToMessageId,
            ReadReceipts = new List<MessageReadReceiptDocument>()
        };
    }

    public static MessageContent ToEntity(MessageContentDocument document)
    {
        if (document == null) return null;

        return new MessageContent
        {
            Text = document.Text,
            Attachment = ToEntity(document.Attachment),
            Location = ToEntity(document.Location),
            VoiceMessage = ToEntity(document.VoiceMessage)
        };
    }

    public static MessageContentDocument ToDocument(MessageContent entity)
    {
        if (entity == null) return null;

        return new MessageContentDocument
        {
            Text = entity.Text,
            Attachment = ToDocument(entity.Attachment),
            Location = ToDocument(entity.Location),
            VoiceMessage = ToDocument(entity.VoiceMessage)
        };
    }

    public static MessageAttachment ToEntity(MessageAttachmentDocument document)
    {
        if (document == null) return null;

        return new MessageAttachment
        {
            Type = document.Type,
            FileUrl = document.FileUrl,
            ThumbnailUrl = document.ThumbnailUrl,
            FileName = document.FileName,
            MimeType = document.MimeType,
            FileSize = document.FileSize,
            Width = document.Width,
            Height = document.Height,
            Duration = document.Duration,
            Metadata = document.Metadata
        };
    }

    public static MessageAttachmentDocument ToDocument(MessageAttachment entity)
    {
        if (entity == null) return null;

        return new MessageAttachmentDocument
        {
            Type = entity.Type,
            FileUrl = entity.FileUrl,
            ThumbnailUrl = entity.ThumbnailUrl,
            FileName = entity.FileName,
            MimeType = entity.MimeType,
            FileSize = entity.FileSize,
            Width = entity.Width,
            Height = entity.Height,
            Duration = entity.Duration,
            Metadata = entity.Metadata
        };
    }

    public static LocationShare ToEntity(LocationShareDocument document)
    {
        if (document == null) return null;

        return new LocationShare
        {
            Latitude = document.Latitude,
            Longitude = document.Longitude,
            Address = document.Address,
            PlaceName = document.PlaceName,
            MapPreviewUrl = document.MapPreviewUrl
        };
    }

    public static LocationShareDocument ToDocument(LocationShare entity)
    {
        if (entity == null) return null;

        return new LocationShareDocument
        {
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            Address = entity.Address,
            PlaceName = entity.PlaceName,
            MapPreviewUrl = entity.MapPreviewUrl
        };
    }

    public static VoiceMessage ToEntity(VoiceMessageDocument document)
    {
        if (document == null) return null;

        return new VoiceMessage
        {
            AudioUrl = document.AudioUrl,
            Duration = document.Duration,
            Waveform = document.Waveform,
            Transcript = document.Transcript
        };
    }

    public static VoiceMessageDocument ToDocument(VoiceMessage entity)
    {
        if (entity == null) return null;

        return new VoiceMessageDocument
        {
            AudioUrl = entity.AudioUrl,
            Duration = entity.Duration,
            Waveform = entity.Waveform,
            Transcript = entity.Transcript
        };
    }
}