namespace Lendme.Core.Entities.PaymentService;

public class Payment
{
    public Guid Id { get; set; }
    public string PaymentNumber { get; set; } // PAY-2024-001234
    public Guid BookingId { get; set; }
    public Guid PayerId { get; set; }
    public Guid RecipientId { get; set; }
        
    // Платежный метод
    public Guid? PaymentMethodId { get; set; } // Ссылка на сохраненный метод
    public PaymentMethodType UsedMethodType { get; set; } // Фактически использованный тип
        
    // Основная информация
    public PaymentType Type { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
        
    // Временные метки
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime? FailedAt { get; set; }
        
    // Детали обработки
    public string FailureReason { get; set; }
    public string ExternalTransactionId { get; set; }
    public string PaymentProvider { get; set; } // Stripe, PayPal, LocalBank
        
    // Навигация
    public PaymentMethod PaymentMethod { get; set; }
    public ICollection<PaymentTransaction> Transactions { get; set; }
}

// Enums
public enum PaymentType 
{ 
    Rental,           // Основная оплата аренды
    Deposit,          // Залог
    Extension,        // Продление аренды
    Penalty,          // Штраф
    DamageCompensation, // Компенсация ущерба
    ServiceFee,       // Комиссия платформы
    Refund,          // Возврат
    PartialRefund    // Частичный возврат
}

public enum PaymentStatus 
{ 
    Created,
    Pending, 
    Processing, 
    Completed, 
    Failed, 
    Cancelled,
    Refunded, 
    PartiallyRefunded,
    Expired
}

public enum PaymentMethodType 
{ 
    Card,           // Банковская карта
    BankAccount,    // Банковский счет
    Cash,           // Наличные
    PayPal,         // PayPal
    ApplePay,       // Apple Pay
    GooglePay,      // Google Pay
    Crypto          // Криптовалюта
}