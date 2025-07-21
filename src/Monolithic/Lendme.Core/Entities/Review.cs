namespace Lendme.Core.Entities;

public class Review
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public int RenterId { get; set; }
    public User Renter { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTimeOffset ReviewDate { get; set; }
}