namespace Lendme.Core.Entities.Booking;

public class Booking
{
    public Guid Id { get; set; }
    public string BookingNumber { get; set; } // RENT-2024-001234
    public Guid ItemId { get; set; }
    public Guid RenterId { get; set; }
    public Guid OwnerId { get; set; }
    
    // Dates
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? PickedUpAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime ExpiresAt { get; set; } // Для автоотмены неоплаченных
    
    // Status
    public BookingStatus Status { get; set; }
    public string CancellationReason { get; set; }
    public CancellationType? CancellationType { get; set; }
    
    // Financial
    public BookingFinancials Financials { get; set; }
    
    // Collections
    public ICollection<BookingStatusHistory> StatusHistory { get; set; }
    public ICollection<BookingPayment> Payments { get; set; }
    public ItemHandover PickupHandover { get; set; }
    public ItemHandover ReturnHandover { get; set; }
    //public BookingExtension Extension { get; set; }
}

// Enums
public enum BookingStatus 
{ 
    Pending, Confirmed, Cancelled, 
    InProgress, Completed, Disputed,
    Expired, Rejected
}

public enum CancellationType
{
    ByRenter,
    ByOwner,
    ByAdmin,
    BySystem,
    Mutual
}