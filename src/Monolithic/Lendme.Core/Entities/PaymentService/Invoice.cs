namespace Lendme.Core.Entities.PaymentService;

public class Invoice
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } // INV-2024-001234
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public InvoiceType Type { get; set; }
    public InvoiceStatus Status { get; set; }
    
    // Даты
    public DateTime IssuedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidAt { get; set; }
    
    // Суммы
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }
    
    // Реквизиты
    public BillingInfo BillingInfo { get; set; }
    //public CompanyInfo CompanyInfo { get; set; }
    
    // Детали
    public List<InvoiceLineItem> LineItems { get; set; }
    //public List<InvoiceTax> Taxes { get; set; }
    
    // Файлы
    public string PdfUrl { get; set; }
    public string XmlUrl { get; set; } // Для электронного документооборота
}


public class InvoiceLineItem
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } // "день", "час", "шт"
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
}

public class BillingInfo
{
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string TaxId { get; set; } // ИИН/БИН
    // TODO: Define address alter
    //public Address Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}

public enum InvoiceType 
{ 
    Rental,         // Счет за аренду
    Deposit,        // Счет за залог
    Commission,     // Счет за комиссию
    Penalty,        // Счет за штраф
    CreditNote      // Кредит-нота
}

public enum InvoiceStatus 
{ 
    Draft, 
    Issued, 
    Sent, 
    Paid, 
    Overdue, 
    Cancelled 
}