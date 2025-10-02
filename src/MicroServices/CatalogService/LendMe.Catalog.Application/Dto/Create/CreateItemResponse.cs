namespace LendMe.Catalog.Application.Dto.Create;

public class CreateItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}