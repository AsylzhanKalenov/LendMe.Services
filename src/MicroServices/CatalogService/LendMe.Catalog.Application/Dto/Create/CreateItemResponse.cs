namespace LendMe.Catalog.Application.Dto.Create;

public class CreateItemResponse
{
    public Guid Id { get; set; }
    public string IdentifyNumber { get; set; }
    public string Title { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}