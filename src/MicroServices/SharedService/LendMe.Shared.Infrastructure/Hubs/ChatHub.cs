using LendMe.Shared.Application.Interfaces.ChatServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace LendMe.Shared.Infrastructure.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IChatService chatService, ILogger<ChatHub> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    public async Task JoinChatGroup(Guid chatId)
    {
        var userId = GetCurrentUserId();
        var hasAccess = await _chatService.HasUserAccessToChatAsync(chatId, userId);
        
        if (hasAccess)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat_{chatId}");
            _logger.LogInformation("User {UserId} joined chat {ChatId}", userId, chatId);
        }
    }

    public async Task LeaveChatGroup(Guid chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chat_{chatId}");
        _logger.LogInformation("User {UserId} left chat {ChatId}", GetCurrentUserId(), chatId);
    }

    public async Task SendTypingIndicator(Guid chatId, bool isTyping)
    {
        var userId = GetCurrentUserId();
        await Clients.OthersInGroup($"chat_{chatId}")
            .SendAsync("UserTyping", chatId, userId, isTyping);
    }

    public async Task MarkMessageAsRead(Guid chatId, Guid messageId)
    {
        var userId = GetCurrentUserId();
        await _chatService.MarkMessageAsReadAsync(messageId, userId);
        
        await Clients.OthersInGroup($"chat_{chatId}")
            .SendAsync("MessageRead", chatId, messageId, userId);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _logger.LogInformation("User {UserId} disconnected from chat hub", GetCurrentUserId());
        await base.OnDisconnectedAsync(exception);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = Context.User?.FindFirst("sub")?.Value ?? 
                         Context.User?.FindFirst("id")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}