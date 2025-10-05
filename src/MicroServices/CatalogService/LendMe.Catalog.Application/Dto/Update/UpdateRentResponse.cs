namespace LendMe.Catalog.Application.Dto.Update;

public class UpdateRentResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}