using LendMe.Shared.Core.Entities.ChatService;

namespace LendMe.Shared.Application.Interfaces.ChatServices;

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
    Task<Core.Entities.ChatService.Chat?> GetChatAsync(Guid chatId);
    Task<Core.Entities.ChatService.Chat> CreateChatAsync(Core.Entities.ChatService.Chat chat);
    Task<List<Core.Entities.ChatService.Chat>> GetUserChatsAsync(Guid userId);
    
    // Участники чата
    Task<List<ChatParticipant>> GetChatParticipantsAsync(Guid chatId);
    Task AddParticipantAsync(Guid chatId, Guid userId, ParticipantRole role);
    Task RemoveParticipantAsync(Guid chatId, Guid userId);
}