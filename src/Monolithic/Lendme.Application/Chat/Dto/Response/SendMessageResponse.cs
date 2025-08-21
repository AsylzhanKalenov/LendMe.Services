using Lendme.Core.Entities.Chat;

namespace Lendme.Application.Chat.Dto.Response;

public class SendMessageResponse
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public MessageType Type { get; set; }
    public MessageContent Content { get; set; }
    public DateTime SentAt { get; set; }
    public MessageStatus Status { get; set; }
}