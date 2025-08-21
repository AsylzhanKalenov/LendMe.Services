using Lendme.Application.Chat.Dto.Response;
using Lendme.Core.Entities.Chat;
using Lendme.Core.Interfaces.Services.ChatServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lendme.Application.Chat.Commands.Create;

public class SendMessageCommand : IRequest<SendMessageResponse>
{
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public MessageType Type { get; set; } = MessageType.Text;
    public MessageContent Content { get; set; }
    public Guid? ReplyToMessageId { get; set; }

    public class Handler : IRequestHandler<SendMessageCommand, SendMessageResponse>
    {
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IChatService chatService,
            IMediator mediator,
            ILogger<Handler> logger)
        {
            _chatService = chatService;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<SendMessageResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Проверяем доступ к чату
                var hasAccess = await _chatService.HasUserAccessToChatAsync(request.ChatId, request.SenderId);
                if (!hasAccess)
                {
                    throw new UnauthorizedAccessException(
                        $"User {request.SenderId} does not have access to chat {request.ChatId}");
                }

                // Создаем сообщение используя существующую структуру Message
                var message = new Message
                {
                    Id = Guid.NewGuid(),
                    ChatId = request.ChatId,
                    SenderId = request.SenderId,
                    Type = request.Type,
                    Content = request.Content,
                    SentAt = DateTime.UtcNow,
                    Status = MessageStatus.Sending,
                    ReplyToMessageId = request.ReplyToMessageId
                };

                // Сохраняем сообщение
                var savedMessage = await _chatService.SaveMessageAsync(message);

                // Публикуем событие для отправки через SignalR
                //await _mediator.Publish(new MessageSentEvent(savedMessage), cancellationToken);

                _logger.LogInformation("Message {MessageId} sent to chat {ChatId} by user {SenderId}",
                    savedMessage.Id, request.ChatId, request.SenderId);

                return new SendMessageResponse
                {
                    Id = savedMessage.Id,
                    ChatId = savedMessage.ChatId,
                    SenderId = savedMessage.SenderId,
                    Type = savedMessage.Type,
                    Content = savedMessage.Content,
                    SentAt = savedMessage.SentAt,
                    Status = savedMessage.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message to chat {ChatId}", request.ChatId);
                throw;
            }
        }
    }
}