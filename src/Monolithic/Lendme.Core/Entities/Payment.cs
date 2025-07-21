namespace Lendme.Core.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string TransactionId { get; set; }
    public bool IsPaid { get; set; }
    public DateTimeOffset PaidDate { get; set; }
}