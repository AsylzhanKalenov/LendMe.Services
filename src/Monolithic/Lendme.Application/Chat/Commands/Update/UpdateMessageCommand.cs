using Lendme.Application.Chat.Dto.Response;
using Lendme.Core.Interfaces.Services.ChatServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lendme.Application.Chat.Commands.Update;

public class UpdateMessageCommand : IRequest<UpdateMessageResponse>
{
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, UpdateMessageResponse>
    {
        private readonly IChatService _chatService;
        private readonly IChatNotificationService _notificationService;
        private readonly ILogger<UpdateMessageCommandHandler> _logger;

        public UpdateMessageCommandHandler(
            IChatService chatService,
            IChatNotificationService notificationService,
            ILogger<UpdateMessageCommandHandler> logger)
        {
            _chatService = chatService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<UpdateMessageResponse> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var message = await _chatService.GetMessageAsync(request.MessageId);
                
                if (message == null)
                {
                    throw new InvalidOperationException($"Message {request.MessageId} not found");
                }

                if (message.SenderId != request.UserId)
                {
                    throw new UnauthorizedAccessException("You can only edit your own messages");
                }

                // Обновляем сообщение
                message.Content.Text = request.Content;
                message.EditedAt = DateTime.UtcNow;
                
                var updatedMessage = await _chatService.UpdateMessageAsync(message);
                
                // Уведомляем об изменении
                await _notificationService.NotifyMessageUpdatedAsync(updatedMessage);

                _logger.LogInformation("Message {MessageId} updated by user {UserId}", 
                    request.MessageId, request.UserId);

                return new UpdateMessageResponse
                {
                    Id = updatedMessage.Id,
                    ChatId = updatedMessage.ChatId,
                    Content = updatedMessage.Content.Text,
                    EditedAt = updatedMessage.EditedAt,
                    IsDeleted = updatedMessage.IsDeleted
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update message {MessageId}", request.MessageId);
                throw;
            }
        }
    }
}
