namespace Lendme.Core.Entities.PaymentService;

public class PaymentFee
{
    public Guid Id { get; set; }
    public FeeType Type { get; set; }
    public string Name { get; set; }
    public decimal Rate { get; set; } // Процент
    public decimal FixedAmount { get; set; } // Фиксированная сумма
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public PaymentMethodType? ApplicableToMethod { get; set; }
    public bool IsActive { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}

public enum FeeType 
{ 
    Platform,       // Комиссия платформы
    Processing,     // Комиссия за обработку платежа
    Currency,       // Комиссия за конвертацию валюты
    Withdrawal      // Комиссия за вывод средств
}