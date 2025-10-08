using Lendme.Core.Entities.Booking;

namespace Lendme.Application.Booking.Dto;

public class BookingPaymentDto
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public PaymentPurpose Purpose { get; set; }
    public PaymentMethod Method { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public PaymentStatus Status { get; set; }
    
    // External payment info
    public string ExternalPaymentId { get; set; }
    public string PaymentProvider { get; set; } // Stripe, PayPal, LocalBank
    
    // Timestamps
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public DateTimeOffset? FailedAt { get; set; }
    
    // Details
    public string FailureReason { get; set; }
    public PaymentMetadata Metadata { get; set; }
    
    // Relations
    public Core.Entities.Booking.Booking Booking { get; set; }
}

public class PaymentMetadata
{
    public string CardLast4 { get; set; }
    public string CardBrand { get; set; }
    public string BankName { get; set; }
    public string TransactionReference { get; set; }
    public Dictionary<string, string> AdditionalData { get; set; }
}
