namespace LendMe.Shared.Application.Chats.Dto.Response;

public class UpdateMessageResponse
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public string Content { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; }
}