namespace Lendme.Core.Entities.Booking;

public class BookingStatusHistory
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public BookingStatus FromStatus { get; set; }
    public BookingStatus ToStatus { get; set; }
    public string Reason { get; set; }
    public string ChangedBy { get; set; } // UserId or "System"
    public DateTimeOffset ChangedAt { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}