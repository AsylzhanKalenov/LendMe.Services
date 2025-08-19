using Lendme.Core.Entities.Booking;
using Lendme.Core.Entities.Profile;

namespace Lendme.Application.Booking.Dto;


public class ItemHandoverDto
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public HandoverType Type { get; set; } // Pickup, Return
    public HandoverStatus Status { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? ActualDate { get; set; }
    
    // Location
    public HandoverLocationDto LocationDto { get; set; }
    
    // Condition check
    public ItemConditionDto ConditionDto { get; set; }
    public List<HandoverPhotoDto> Photos { get; set; }
    public string Notes { get; set; }
    
    // Verification
    public HandoverVerificationDto VerificationDto { get; set; }
    
    // Relations
    public Core.Entities.Booking.Booking Booking { get; set; }
}

public class HandoverVerificationDto
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

public class HandoverLocationDto
{
    public HandoverMethod Method { get; set; } // Pickup, Delivery, Shipping
    public Address Address { get; set; }
    public string MeetingPoint { get; set; }
    public string Instructions { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public GeoLocation Coordinates { get; set; }
}

public class ItemConditionDto
{
    public ConditionGrade Grade { get; set; } // Excellent, Good, Fair, Poor
    public string Description { get; set; }
    public List<DamageReportDto> Damages { get; set; }
    public bool HasAllAccessories { get; set; }
    public List<string> MissingAccessories { get; set; }
    public bool IsClean { get; set; }
    public bool IsFunctional { get; set; }
}

public class DamageReportDto
{
    public Guid Id { get; set; }
    public DamageType Type { get; set; }
    public DamageSeverity Severity { get; set; }
    public string Description { get; set; }
    public List<string> PhotoUrls { get; set; }
    public decimal EstimatedCost { get; set; }
    public bool IsPreExisting { get; set; }
}

public class HandoverPhotoDto
{
    public Guid Id { get; set; }
    public string PhotoUrl { get; set; }
    public string ThumbnailUrl { get; set; }
    public PhotoCategory Category { get; set; } // General, Damage, Accessory
    public string Description { get; set; }
    public DateTime TakenAt { get; set; }
    public PhotoMetadataDto MetadataDto { get; set; }
    
    // AI Analysis
    //public AiAnalysisResultDto AiAnalysis { get; set; }
}

public class AiAnalysisResultDto
{
    public string AnalysisId { get; set; }
    public DateTime AnalyzedAt { get; set; }
    public List<DetectedIssueDto> DetectedIssues { get; set; }
    public double ConfidenceScore { get; set; }
    public string Summary { get; set; }
}

public class PhotoMetadataDto
{
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Dictionary<string, string> ExifData { get; set; }
    public GeoLocation Location { get; set; }
}