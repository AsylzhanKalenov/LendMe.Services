namespace Lendme.Core.Entities.PaymentService;

public class PaymentTransaction
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Description { get; set; }
    
    // Внешние идентификаторы
    public string ExternalId { get; set; }
    public string AuthorizationCode { get; set; }
    
    // Для возвратов
    public Guid? OriginalTransactionId { get; set; }
    public string RefundReason { get; set; }
    
    // Комиссии
    public decimal? ProcessingFee { get; set; }
    public decimal? PlatformFee { get; set; }
    public decimal NetAmount { get; set; }
    
    // Навигация
    public Payment Payment { get; set; }
}

public enum TransactionType 
{ 
    Authorization,   // Блокировка средств
    Capture,        // Списание заблокированных средств
    Charge,         // Прямое списание
    Refund,         // Возврат
    PartialRefund,  // Частичный возврат
    Void,           // Отмена транзакции
    Adjustment      // Корректировка
}

public enum TransactionStatus 
{ 
    Pending, 
    Processing, 
    Completed, 
    Failed, 
    Cancelled 
}