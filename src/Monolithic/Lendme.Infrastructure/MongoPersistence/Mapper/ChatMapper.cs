using Lendme.Core.Entities.Chat;
using Lendme.Infrastructure.MongoPersistence.Documents.ChatDocs;

namespace Lendme.Infrastructure.MongoPersistence.Mapper;

public static class ChatMapper
{
    public static Chat ToEntity(ChatDocument document)
    {
        if (document == null) return null;

        return new Chat
        {
            Id = document.Id,
            Type = document.Type,
            ItemId = document.ItemId,
            BookingId = document.BookingId,
            Participants = document.Participants?.Select(ToEntity).ToList() ?? new List<ChatParticipant>(),
            CreatedAt = document.CreatedAt,
            LastMessageAt = document.LastMessageAt,
            ArchivedAt = document.ArchivedAt,
            Status = document.Status,
            Settings = ToEntity(document.Settings),
            MessageCount = document.MessageCount,
            UnreadCount = document.UnreadCount
        };
    }

    public static ChatDocument ToDocument(Chat entity)
    {
        if (entity == null) return null;

        return new ChatDocument
        {
            Id = entity.Id,
            Type = entity.Type,
            ItemId = entity.ItemId,
            BookingId = entity.BookingId,
            Participants = entity.Participants?.Select(ToDocument).ToList() ?? new List<ChatParticipantDocument>(),
            CreatedAt = entity.CreatedAt,
            LastMessageAt = entity.LastMessageAt,
            ArchivedAt = entity.ArchivedAt,
            Status = entity.Status,
            Settings = ToDocument(entity.Settings),
            MessageCount = entity.MessageCount,
            UnreadCount = entity.UnreadCount
        };
    }

    public static ChatParticipant ToEntity(ChatParticipantDocument document)
    {
        if (document == null) return null;

        return new ChatParticipant
        {
            Id = document.Id,
            UserId = document.UserId,
            Role = document.Role,
            JoinedAt = document.JoinedAt,
            LeftAt = document.LeftAt,
            IsActive = document.IsActive,
            IsMuted = document.IsMuted,
            MutedUntil = document.MutedUntil,
            LastSeenAt = document.LastReadMessageAt,
            LastReadMessageId = document.LastReadMessageId
        };
    }

    public static ChatParticipantDocument ToDocument(ChatParticipant entity)
    {
        if (entity == null) return null;

        return new ChatParticipantDocument
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Role = entity.Role,
            JoinedAt = entity.JoinedAt,
            LeftAt = entity.LeftAt,
            IsActive = entity.IsActive,
            IsMuted = entity.IsMuted,
            MutedUntil = entity.MutedUntil,
            LastReadMessageAt = entity.LastSeenAt,
            LastReadMessageId = entity.LastReadMessageId
        };
    }

    public static ChatSettings ToEntity(ChatSettingsDocument document)
    {
        if (document == null) return null;

        return new ChatSettings
        {
            IsReadOnly = document.IsReadOnly,
            AllowFiles = document.AllowFiles,
            AllowVoiceMessages = document.AllowVoiceMessages,
            AllowLocation = document.AllowLocation,
            ShowTypingIndicator = document.ShowTypingIndicator,
            AutoArchiveEnabled = document.AutoArchiveEnabled,
            AutoArchiveDays = document.AutoArchiveDays,
            MaxFileSize = document.MaxFileSize,
            AllowedFileTypes = document.AllowedFileTypes
        };
    }

    public static ChatSettingsDocument ToDocument(ChatSettings entity)
    {
        if (entity == null) return null;

        return new ChatSettingsDocument
        {
            IsReadOnly = entity.IsReadOnly,
            AllowFiles = entity.AllowFiles,
            AllowVoiceMessages = entity.AllowVoiceMessages,
            AllowLocation = entity.AllowLocation,
            ShowTypingIndicator = entity.ShowTypingIndicator,
            AutoArchiveEnabled = entity.AutoArchiveEnabled,
            AutoArchiveDays = entity.AutoArchiveDays,
            MaxFileSize = entity.MaxFileSize,
            AllowedFileTypes = entity.AllowedFileTypes
        };
    }
}