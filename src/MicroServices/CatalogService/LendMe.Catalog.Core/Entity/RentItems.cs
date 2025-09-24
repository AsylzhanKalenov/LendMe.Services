namespace LendMe.Catalog.Core.Entity;

public class RentItems
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid RentId { get; set; }
    public Rent Rent { get; set; }
    public Item Item { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}