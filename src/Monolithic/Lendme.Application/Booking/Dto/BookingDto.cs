using Lendme.Core.Entities.Booking;

namespace Lendme.Application.Booking.Dto;

public class BookingDto
{
    public Guid Id { get; set; }
    public string BookingNumber { get; set; } // RENT-2024-001234
    public Guid RentalId { get; set; }
    public Guid ItemId { get; set; }
    public Guid RenterId { get; set; }
    public Guid OwnerId { get; set; }
    
    // Dates
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ConfirmedAt { get; set; }
    public DateTimeOffset? PickedUpAt { get; set; }
    public DateTimeOffset? ReturnedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; } // Для автоотмены неоплаченных
    
    // Status
    public BookingStatus Status { get; set; }
    public string CancellationReason { get; set; }
    public CancellationType? CancellationType { get; set; }
    
    // Financial
    public BookingFinancialsDto Financials { get; set; }
    
    // Collections
    public ICollection<BookingStatusHistoryDto> StatusHistory { get; set; }
    public ICollection<BookingPaymentDto> Payments { get; set; }
    public ItemHandoverDto PickupHandover { get; set; }
    public ItemHandoverDto ReturnHandover { get; set; }
}