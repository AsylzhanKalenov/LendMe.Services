using Lendme.Core.Entities.Chat;

namespace Lendme.Core.Interfaces.Services.ChatServices;

public interface IChatNotificationService
{
    Task NotifyNewMessageAsync(Message message);
    Task NotifyMessageUpdatedAsync(Message message);
    Task NotifyUserJoinedChatAsync(Guid chatId, Guid userId, string userName);
    Task NotifyUserLeftChatAsync(Guid chatId, Guid userId, string userName);
}