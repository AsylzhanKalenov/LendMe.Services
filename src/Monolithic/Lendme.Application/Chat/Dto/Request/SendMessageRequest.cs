using Lendme.Core.Entities.Chat;

namespace Lendme.Application.Chat.Dto.Request;

public class SendMessageRequest
{
    public string Content { get; set; }
    public MessageType Type { get; set; } = MessageType.Text;
    //public List<ChatMessageAttachment> Attachments { get; set; }
}