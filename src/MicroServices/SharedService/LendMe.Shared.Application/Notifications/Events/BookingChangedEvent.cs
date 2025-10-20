namespace LendMe.Shared.Application.Notifications.Events;

public class BookingChangedEvent
{
    public Guid BookingId { get; set; }
    public string BookingNumber { get; set; }
    public Guid RentId { get; set; }
    public Guid ItemId { get; set; }
    public Guid RenterId { get; set; }
    public Guid OwnerId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}