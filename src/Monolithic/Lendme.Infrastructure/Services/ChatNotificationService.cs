using Microsoft.AspNetCore.SignalR;
using Lendme.Core.Entities.Chat;
using Lendme.Core.Interfaces.Services.ChatServices;
using Lendme.Infrastructure.Hubs;
using Microsoft.Extensions.Logging;

namespace Lendme.Infrastructure.Services;

public class ChatNotificationService : IChatNotificationService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<ChatNotificationService> _logger;

    public ChatNotificationService(IHubContext<ChatHub> hubContext, ILogger<ChatNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotifyNewMessageAsync(Message message) // Используем Message вместо ChatMessage
    {
        try
        {
            await _hubContext.Clients.Group($"chat_{message.ChatId}")
                .SendAsync("NewMessage", new
                {
                    Id = message.Id,
                    ChatId = message.ChatId,
                    SenderId = message.SenderId,
                    Type = message.Type.ToString(),
                    Content = message.Content,
                    SentAt = message.SentAt,
                    Status = message.Status.ToString(),
                    ReplyToMessageId = message.ReplyToMessageId
                });

            _logger.LogInformation("New message notification sent for chat {ChatId}", message.ChatId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send new message notification for chat {ChatId}", message.ChatId);
        }
    }

    public async Task NotifyMessageUpdatedAsync(Message message)
    {
        try
        {
            await _hubContext.Clients.Group($"chat_{message.ChatId}")
                .SendAsync("MessageUpdated", new
                {
                    Id = message.Id,
                    ChatId = message.ChatId,
                    Content = message.Content,
                    EditedAt = message.EditedAt,
                    DeletedAt = message.DeletedAt
                });

            _logger.LogInformation("Message updated notification sent for chat {ChatId}", message.ChatId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send message updated notification for chat {ChatId}", message.ChatId);
        }
    }

    public async Task NotifyUserJoinedChatAsync(Guid chatId, Guid userId, string userName)
    {
        try
        {
            await _hubContext.Clients.Group($"chat_{chatId}")
                .SendAsync("UserJoined", chatId, userId, userName);

            _logger.LogInformation("User joined notification sent for chat {ChatId}", chatId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send user joined notification for chat {ChatId}", chatId);
        }
    }

    public async Task NotifyUserLeftChatAsync(Guid chatId, Guid userId, string userName)
    {
        try
        {
            await _hubContext.Clients.Group($"chat_{chatId}")
                .SendAsync("UserLeft", chatId, userId, userName);

            _logger.LogInformation("User left notification sent for chat {ChatId}", chatId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send user left notification for chat {ChatId}", chatId);
        }
    }

}