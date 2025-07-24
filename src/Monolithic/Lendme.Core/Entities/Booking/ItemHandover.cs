using Lendme.Core.Entities.Profile;

namespace Lendme.Core.Entities.Booking;


public class ItemHandover
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public HandoverType Type { get; set; } // Pickup, Return
    public HandoverStatus Status { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? ActualDate { get; set; }
    
    // Location
    public HandoverLocation Location { get; set; }
    
    // Condition check
    public ItemCondition Condition { get; set; }
    public List<HandoverPhoto> Photos { get; set; }
    public string Notes { get; set; }
    
    // Verification
    public HandoverVerification Verification { get; set; }
    
    // Relations
    public Booking Booking { get; set; }
}

public class HandoverVerification
{
    public bool IsOwnerPresent { get; set; }
    public bool IsRenterPresent { get; set; }
    public string OwnerSignature { get; set; } // Base64
    public string RenterSignature { get; set; } // Base64
    public DateTime? SignedAt { get; set; }
    public string VerificationCode { get; set; } // SMS/Email код подтверждения
    public bool IsDisputed { get; set; }
    public string DisputeReason { get; set; }
}

public class HandoverLocation
{
    public HandoverMethod Method { get; set; } // Pickup, Delivery, Shipping
    public Address Address { get; set; }
    public string MeetingPoint { get; set; }
    public string Instructions { get; set; }
    public GeoLocation Coordinates { get; set; }
}

public class ItemCondition
{
    public ConditionGrade Grade { get; set; } // Excellent, Good, Fair, Poor
    public string Description { get; set; }
    public List<DamageReport> Damages { get; set; }
    public bool HasAllAccessories { get; set; }
    public List<string> MissingAccessories { get; set; }
    public bool IsClean { get; set; }
    public bool IsFunctional { get; set; }
}

public class DamageReport
{
    public Guid Id { get; set; }
    public DamageType Type { get; set; }
    public DamageSeverity Severity { get; set; }
    public string Description { get; set; }
    public List<string> PhotoUrls { get; set; }
    public decimal EstimatedCost { get; set; }
    public bool IsPreExisting { get; set; }
}

public class HandoverPhoto
{
    public Guid Id { get; set; }
    public string PhotoUrl { get; set; }
    public string ThumbnailUrl { get; set; }
    public PhotoCategory Category { get; set; } // General, Damage, Accessory
    public string Description { get; set; }
    public DateTime TakenAt { get; set; }
    public PhotoMetadata Metadata { get; set; }
    
    // AI Analysis
    public AiAnalysisResult AiAnalysis { get; set; }
}

public class AiAnalysisResult
{
    public string AnalysisId { get; set; }
    public DateTime AnalyzedAt { get; set; }
    public List<DetectedIssue> DetectedIssues { get; set; }
    public double ConfidenceScore { get; set; }
    public string Summary { get; set; }
}

public class PhotoMetadata
{
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Dictionary<string, string> ExifData { get; set; }
    public GeoLocation Location { get; set; }
}

public enum HandoverType { Pickup, Return }
public enum HandoverStatus { Scheduled, InProgress, Completed, Disputed }
public enum HandoverMethod { Pickup, Delivery, Shipping }

public enum ConditionGrade { Excellent, Good, Fair, Poor, Damaged }
public enum DamageType { Scratch, Dent, Crack, Stain, Burn, Tear, Other }
public enum DamageSeverity { Minor, Moderate, Major, Critical }
public enum PhotoCategory { General, Damage, Accessory, Document }