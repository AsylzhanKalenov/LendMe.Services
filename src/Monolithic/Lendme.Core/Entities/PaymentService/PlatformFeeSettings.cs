namespace Lendme.Core.Entities.PaymentService;

public class PlatformFeeSettings
{
    public Guid Id { get; set; }
    public decimal StandardFeeRate { get; set; } = 0.20m; // 20% стандартная комиссия
    public decimal MinimumFee { get; set; } = 100; // Минимум 100 тг
    public decimal ProcessingFeeRate { get; set; } = 0.029m; // 2.9% за обработку платежа
    public decimal ProcessingFeeFixed { get; set; } = 50; // + 50 тг фиксировано
    
    // Специальные ставки для категорий
    public Dictionary<string, decimal> CategoryFeeRates { get; set; }
    
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}