namespace Lendme.Core.Entities.PaymentService;

public class PaymentMethod
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public PaymentMethodType Type { get; set; }
    public string DisplayName { get; set; } // "Visa ****4242"
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }

    // ВАЖНО: Мы НЕ храним реальные данные карт!
    // Только метаданные для отображения пользователю
    public CardMetadata CardMetadata { get; set; }

    // Банковский счет (только метаданные)
    public BankAccountMetadata BankMetadata { get; set; }

    // Цифровой кошелек
    public DigitalWalletMetadata WalletMetadata { get; set; }

    // Токенизация - это главное!
    public PaymentTokenInfo TokenInfo { get; set; }

    // Метаданные
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public int UsageCount { get; set; }

    // Навигация
    public ICollection<Payment> Payments { get; set; }
}

// КРИТИЧЕСКИ ВАЖНО: Токенизация для безопасности
public class PaymentTokenInfo
{
    // Токены от платежных провайдеров - это единственное что мы храним!
    public string PaymentToken { get; set; } // Токен от Stripe/PayPal
    public string CustomerId { get; set; } // Customer ID в платежной системе
    public string PaymentMethodId { get; set; } // Payment Method ID в Stripe
    public string Provider { get; set; } // "Stripe", "PayPal", "CloudPayments"
    public string TokenType { get; set; } // "card", "bank_account", "source"

    // Дополнительная безопасность
    public string Fingerprint { get; set; } // Уникальный отпечаток карты от провайдера
    public DateTime TokenExpiresAt { get; set; } // Срок действия токена
}

// Только метаданные карты для UI (НЕ реальные данные!)
public class CardMetadata
{
    public string Last4 { get; set; } // "4242" - только последние 4 цифры
    public string Brand { get; set; } // "Visa", "Mastercard", "Amex"
    public int? ExpiryMonth { get; set; } // Можно хранить
    public int? ExpiryYear { get; set; } // Можно хранить
    public string Country { get; set; } // Страна выпуска (2 буквы)
    public string Funding { get; set; } // "credit", "debit", "prepaid"

    // НЕ ХРАНИМ:
    // - Полный номер карты
    // - CVV/CVC код
    // - PIN код
    // - Имя держателя карты (если не требуется для бизнеса)
}

// Метаданные банковского счета
public class BankAccountMetadata
{
    public string BankName { get; set; }
    public string AccountNumberLast4 { get; set; } // Только последние 4 цифры
    public string AccountType { get; set; } // "checking", "savings"

    // НЕ ХРАНИМ полные номера счетов
    // Используем токены от платежного провайдера
}

// Метаданные цифрового кошелька
// Метаданные цифрового кошелька
public class DigitalWalletMetadata
{
    public string WalletType { get; set; } // "PayPal", "ApplePay", "GooglePay"
    public string Email { get; set; } // Для PayPal (можно хранить)
    public string WalletDisplayName { get; set; } // "john@example.com"
}