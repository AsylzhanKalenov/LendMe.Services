using Lendme.Core.Entities.Chat;

namespace Lendme.Application.Chat.Dto.Response;

public class GetChatMessagesResponse
{
    public List<Message> Messages { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage { get; set; }
}