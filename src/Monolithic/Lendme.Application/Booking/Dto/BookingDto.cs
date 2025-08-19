using Lendme.Core.Entities.Booking;

namespace Lendme.Application.Booking.Dto;

public class BookingDto
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
    public BookingFinancialsDto Financials { get; set; }
    
    // Collections
    public ICollection<BookingStatusHistoryDto> StatusHistory { get; set; }
    public ICollection<BookingPaymentDto> Payments { get; set; }
    public ItemHandoverDto PickupHandover { get; set; }
    public ItemHandoverDto ReturnHandover { get; set; }
}