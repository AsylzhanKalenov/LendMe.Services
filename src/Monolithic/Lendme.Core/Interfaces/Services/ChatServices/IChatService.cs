using Lendme.Core.Entities.Chat;

namespace Lendme.Core.Interfaces.Services.ChatServices;

public interface IChatService
{
    // Проверка доступа
    Task<bool> HasUserAccessToChatAsync(Guid chatId, Guid userId);
    
    // Работа с сообщениями (используем Message вместо ChatMessage)
    Task<Message> SaveMessageAsync(Message message);
    Task<Message?> GetMessageAsync(Guid messageId);
    Task<Message> UpdateMessageAsync(Message message);
    Task<bool> DeleteMessageAsync(Guid messageId, Guid userId);
    
    // Получение сообщений
    Task<List<Message>> GetChatMessagesAsync(Guid chatId, int page, int pageSize);
    Task<int> GetChatMessageCountAsync(Guid chatId);
    
    // Статус прочтения
    Task MarkMessageAsReadAsync(Guid messageId, Guid userId);
    
    // Работа с чатами
    Task<Chat?> GetChatAsync(Guid chatId);
    Task<Chat> CreateChatAsync(Chat chat);
    Task<List<Chat>> GetUserChatsAsync(Guid userId);
    
    // Участники чата
    Task<List<ChatParticipant>> GetChatParticipantsAsync(Guid chatId);
    Task AddParticipantAsync(Guid chatId, Guid userId, ParticipantRole role);
    Task RemoveParticipantAsync(Guid chatId, Guid userId);
}