namespace Lendme.Core.Entities.Booking;

public class BookingPayment
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
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    
    // Details
    public string FailureReason { get; set; }
    public PaymentMetadata Metadata { get; set; }
    
    // Relations
    public Booking Booking { get; set; }
}

public class PaymentMetadata
{
    public string CardLast4 { get; set; }
    public string CardBrand { get; set; }
    public string BankName { get; set; }
    public string TransactionReference { get; set; }
    public Dictionary<string, string> AdditionalData { get; set; }
}

public enum PaymentMethod
{
    Card,
    BankTransfer,
    Cash,
    DigitalWallet,
    PayPal,
    Stripe
}

public enum PaymentPurpose
{
    Rental,           // Основная оплата
    Deposit,          // Залог
    Extension,        // Продление
    Penalty,          // Штраф
    DamageCompensation, // Компенсация ущерба
    Refund           // Возврат
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Refunded,
    PartiallyRefunded,
    Cancelled
}