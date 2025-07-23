namespace Lendme.Core.Entities.Profile;

public class VerificationDocument
{
    public Guid Id { get; set; }
    public DocumentType Type { get; set; }
    public string DocumentNumber { get; set; } // Encrypted
    public DocumentImages Images { get; set; }
    public VerificationStatus Status { get; set; }
    public string RejectionReason { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string ProcessedBy { get; set; } // AdminId or "AI"
}

public enum DocumentType
{
    IDCard, Passport
}

public enum VerificationStatus
{
    Pending, Approved, Rejected
}