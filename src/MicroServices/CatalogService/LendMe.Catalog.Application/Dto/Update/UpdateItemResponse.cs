namespace LendMe.Catalog.Application.Dto.Update;

public class UpdateItemResponse
{
    public Guid Id { get; set; }
    public string IdentifyNumber { get; set; }
    public string Title { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}