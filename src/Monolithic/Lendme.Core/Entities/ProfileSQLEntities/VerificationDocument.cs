using System.ComponentModel.DataAnnotations.Schema;

namespace Lendme.Core.Entities.ProfileSQLEntities;

public class VerificationDocument
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public DocumentType Type { get; set; }
    public string DocumentNumber { get; set; } // Encrypted
    public DocumentStatus Status { get; set; }
    
    // Изображения
    public string FrontImageUrl { get; set; }
    public string BackImageUrl { get; set; }
    public string SelfieWithDocUrl { get; set; }
    
    // Обработка
    public DateTime SubmittedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string ProcessedBy { get; set; } // AdminId or "AI"
    public string RejectionReason { get; set; }
    
    // Метаданные в JSON
    [Column(TypeName = "jsonb")]
    public Dictionary<string, object> ExtractedData { get; set; }
    
    // Навигация
    public UserProfile Profile { get; set; }
}


public enum DocumentType { Passport, IDCard, DriverLicense, UtilityBill }
public enum DocumentStatus { Pending, Approved, Rejected, Expired }