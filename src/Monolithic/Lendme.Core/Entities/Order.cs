using Lendme.Core.Entities.Catalog;
using Lendme.Core.Entities.Profile;

namespace Lendme.Core.Entities;

public class Order
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public int RenterId { get; set; }
    public UserProfile Renter { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public OrderStatus Status { get; set; }
}

public enum OrderStatus
{
    Pending,
    InProgress,
    Completed,
    Cancelled
}