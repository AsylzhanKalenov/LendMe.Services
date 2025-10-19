using LendMe.Shared.Core.Entities.ChatService;

namespace LendMe.Shared.Application.Interfaces.ChatServices;

public interface IChatNotificationService
{
    Task NotifyNewMessageAsync(Message message);
    Task NotifyMessageUpdatedAsync(Message message);
    Task NotifyUserJoinedChatAsync(Guid chatId, Guid userId, string userName);
    Task NotifyUserLeftChatAsync(Guid chatId, Guid userId, string userName);
}