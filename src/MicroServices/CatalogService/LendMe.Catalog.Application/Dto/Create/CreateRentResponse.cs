namespace LendMe.Catalog.Application.Dto.Create;

public class CreateRentResponse
{
    public Guid Id { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}