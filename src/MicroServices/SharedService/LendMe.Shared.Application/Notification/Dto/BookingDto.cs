namespace LendMe.Shared.Application.Notification.Dto;

public class BookingDto
{
    public Guid Id { get; set; }
    public string BookingNumber { get; set; } // RENT-2024-001234
    public Guid RentId { get; set; }
    public Guid ItemId { get; set; }
    public Guid RenterId { get; set; }
    public Guid OwnerId { get; set; }

    // Dates
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset DeletedAt { get; set; }
    public DateTimeOffset? ConfirmedAt { get; set; }
    public DateTimeOffset? PickedUpAt { get; set; }
    public DateTimeOffset? ReturnedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; } // Для автоотмены неоплаченных

    // Status
    public BookingStatus Status { get; set; }
    public string CancellationReason { get; set; }
    public CancellationType? CancellationType { get; set; }
}

public enum BookingStatus 
{ 
    DRAFT, HOLD_PENDING, AWAIT_OWNER_CONFIRMATION, RECEIPT_UPLOADED, 
    CONFIRMED_READY, CONFIRMED_RENTER, IN_RENTAL, RETURN_PENDING, COMPLETED, DISPUTE_OPEN, 
    DISPUTE_RESOLVED, CANCELLED_BY_RENTER, CANCELLED_BY_OWNER, EXPIRED_HOLD 
}

public enum CancellationType
{
    ByRenter,
    ByOwner,
    ByAdmin,
    BySystem,
    Mutual
}