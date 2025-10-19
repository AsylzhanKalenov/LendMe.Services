using LendMe.Shared.Core.Entities.ChatService;

namespace LendMe.Shared.Application.Chat.Dto.Request;

public class SendMessageRequest
{
    public string Content { get; set; }
    public MessageType Type { get; set; } = MessageType.Text;
    //public List<ChatMessageAttachment> Attachments { get; set; }
}