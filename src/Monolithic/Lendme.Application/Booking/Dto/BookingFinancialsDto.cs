namespace Lendme.Application.Booking.Dto;

public class BookingFinancialsDto
{
    // Основные суммы
    public decimal ItemPrice { get; set; }
    public int RentalDays { get; set; }
    public decimal DailyRate { get; set; }
    public decimal SubTotal { get; set; }
    
    // Дополнительные сборы
    // public decimal ServiceFee { get; set; }
    // public decimal ServiceFeeRate { get; set; } // Процент от SubTotal
    // public decimal InsuranceFee { get; set; }
    // public decimal DeliveryFee { get; set; }
    
    // Скидки
    public decimal DiscountAmount { get; set; }
    public string DiscountCode { get; set; }
    public decimal DiscountPercentage { get; set; }
    
    // Итоговые суммы
    public decimal TotalAmount { get; set; }
    public decimal DepositAmount { get; set; }
    public string Currency { get; set; } = "KZT";
    
    // Штрафы и возвраты
    public decimal PenaltyAmount { get; set; }
    public decimal RefundAmount { get; set; }
    public decimal DamageCompensation { get; set; }
}