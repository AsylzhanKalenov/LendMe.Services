using LendMe.Shared.Application.Interfaces.ChatServices;
using LendMe.Shared.Core.Entities.ChatService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LendMe.Shared.Application.Chats.EventHandler;

public class MessageSentEvent : INotification
{
    public Message Message { get; }
    public DateTime Timestamp { get; }

    public MessageSentEvent(Message message)
    {
        Message = message;
        Timestamp = DateTime.UtcNow;
    }

    public class MessageSentEventHandler : INotificationHandler<MessageSentEvent>
    {
        private readonly IChatNotificationService _notificationService;
        private readonly ILogger<MessageSentEventHandler> _logger;

        public MessageSentEventHandler(
            IChatNotificationService notificationService,
            ILogger<MessageSentEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(MessageSentEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _notificationService.NotifyNewMessageAsync(notification.Message);
                _logger.LogInformation("SignalR notification sent for message {MessageId}", notification.Message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SignalR notification for message {MessageId}",
                    notification.Message.Id);
                // Не пробрасываем исключение, чтобы не нарушить основной поток
            }
        }
    }
}
