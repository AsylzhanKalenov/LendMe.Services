namespace Lendme.Core.Entities.ReviewService;

public class Review
{
    // Mongo
    //public ObjectId Id { get; set; }
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Guid ItemId { get; set; }
    public Guid ReviewerId { get; set; }
    public Guid RevieweeId { get; set; }
    public ReviewType Type { get; set; } // ItemReview, UserReview
    
    public int Rating { get; set; } // 1-5
    public string Title { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Detailed ratings
    public DetailedRatings Ratings { get; set; }
    
    // Media
    public List<ReviewPhoto> Photos { get; set; }
    
    // Interaction
    public int HelpfulCount { get; set; }
    public List<string> HelpfulVoters { get; set; } // User IDs
    public bool IsVerifiedRental { get; set; }
    
    // Response from owner
    public OwnerResponse Response { get; set; }
    
    // Moderation
    public ModerationStatus Status { get; set; }
    public string ModerationNotes { get; set; }
}

public class DetailedRatings
{
    // For items
    public int? QualityRating { get; set; }
    public int? AccuracyRating { get; set; }
    public int? ValueRating { get; set; }
    public int? ConditionRating { get; set; }
    
    // For users
    public int? CommunicationRating { get; set; }
    public int? ReliabilityRating { get; set; }
    public int? FlexibilityRating { get; set; }
}

public class OwnerResponse
{
    public string Comment { get; set; }
    public DateTime RespondedAt { get; set; }
    public bool IsEdited { get; set; }
}

public class ReviewPhoto
{
    public string Url { get; set; }
    public string Caption { get; set; }
    public int Order { get; set; }
}

// Enums
public enum ReviewType { ItemReview, RenterReview, OwnerReview }
public enum ModerationStatus { Pending, Approved, Rejected, Flagged }
public enum EntityType { Item, User }