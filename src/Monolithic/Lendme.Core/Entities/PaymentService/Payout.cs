using Lendme.Core.Entities.Profile;

namespace Lendme.Core.Entities.PaymentService;

public class Payout
{
    public Guid Id { get; set; }
    public string PayoutNumber { get; set; } // POUT-2024-001234
    public Guid UserId { get; set; } // Владелец
    public Guid BookingId { get; set; } // За какое бронирование выплата
    public Guid PaymentId { get; set; } // Связь с платежом арендатора
    
    // Метод выплаты
    public Guid PayoutMethodId { get; set; }
    public PayoutMethodType MethodType { get; set; }
    
    // Суммы
    public decimal BookingAmount { get; set; } // Сумма бронирования
    public decimal PlatformFee { get; set; } // Комиссия платформы (20%)
    public decimal ProcessingFee { get; set; } // Комиссия за обработку платежа
    public decimal NetAmount { get; set; } // Итого к выплате владельцу
    public string Currency { get; set; }
    
    // Статус
    public PayoutStatus Status { get; set; }
    
    // Временные метки
    public DateTime CreatedAt { get; set; } // Когда создана выплата
    public DateTime? ProcessedAt { get; set; } // Когда начата обработка
    public DateTime? CompletedAt { get; set; } // Когда деньги получены
    public DateTime? FailedAt { get; set; }
    
    // Детали обработки
    public string ExternalTransactionId { get; set; } // ID в платежной системе
    public string FailureReason { get; set; }
    
    // Навигация
    //public Booking Booking { get; set; }
    public Payment Payment { get; set; }
    public PayoutMethod PayoutMethod { get; set; }
}

// Метод получения денег владельцем
public class PayoutMethod
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public PayoutMethodType Type { get; set; }
    public string DisplayName { get; set; } // "Kaspi Gold ****4242"
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    
    // Банковская карта (самый популярный способ)
    public CardPayoutInfo CardInfo { get; set; }
    
    // Банковский счет
    public BankAccountInfo BankAccountInfo { get; set; }
    
    // Электронный кошелек
    public DigitalWalletInfo WalletInfo { get; set; }
    
    // Метаданные
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
}

public class PayoutItem
{
    public Guid Id { get; set; }
    public Guid PayoutId { get; set; }
    public Guid BookingId { get; set; }
    public string Description { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal NetAmount { get; set; }
    public DateTime EarnedAt { get; set; }
}

public class CardPayoutInfo
{
    public string CardNumberMasked { get; set; } // ****4242
    public string CardBrand { get; set; } // Visa, Mastercard, Kaspi
    public string CardholderName { get; set; }
    public string BankName { get; set; } // Kaspi Bank, Halyk Bank
    
    // Токен для безопасных выплат
    public string PayoutToken { get; set; } // Токен от платежной системы
    public string TokenProvider { get; set; } // Stripe, CloudPayments
}

public class BankAccountInfo
{
    public string AccountHolderName { get; set; }
    public string BankName { get; set; }
    public string IBAN { get; set; } // KZ75722S000012345678
    public string AccountNumberMasked { get; set; } // ****5678
}

public class DigitalWalletInfo  
{
    public string WalletType { get; set; } // Kaspi, PayPal, YooMoney
    public string WalletId { get; set; } // Номер телефона или email
    public string WalletDisplayName { get; set; } // +7 705 *** **42
}

public class PayoutBankDetails
{
    public string AccountHolderName { get; set; }
    public string BankName { get; set; }
    public string AccountNumber { get; set; } // Зашифрованный
    public string IBAN { get; set; }
    public string BIC { get; set; }
    public string BranchCode { get; set; }
    public Address BankAddress { get; set; }
}

public enum PayoutStatus 
{ 
    Created,       // Создана после оплаты арендатором
    Processing,    // В процессе перевода
    Completed,     // Деньги получены владельцем
    Failed,        // Ошибка перевода
    Retry,         // Повторная попытка
    Blocked        // Заблокировано (спор, проверка)
}

public enum PayoutMethodType
{
    KaspiCard,      // Kaspi Gold (самый популярный в КЗ)
    BankCard,       // Visa/Mastercard других банков
    BankAccount,    // Банковский счет (IBAN)
    DigitalWallet   // Электронные кошельки
}