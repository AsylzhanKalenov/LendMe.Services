using LendMe.Shared.Core.Entities.ReviewService;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LendMe.Shared.Infrastructure.MongoPersistence.Documents.ReviewDocs;

[BsonCollection("reviews")]
public class ReviewDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    [BsonElement("bookingId")]
    [BsonRepresentation(BsonType.String)]
    public Guid BookingId { get; set; }
    
    [BsonElement("itemId")]
    [BsonRepresentation(BsonType.String)]
    public Guid ItemId { get; set; }
    
    [BsonElement("reviewerId")]
    [BsonRepresentation(BsonType.String)]
    public Guid ReviewerId { get; set; }
    
    [BsonElement("revieweeId")]
    [BsonRepresentation(BsonType.String)]
    public Guid RevieweeId { get; set; }
    
    [BsonElement("type")]
    [BsonRepresentation(BsonType.String)]
    public ReviewType Type { get; set; }
    
    [BsonElement("rating")]
    public int Rating { get; set; }
    
    [BsonElement("title")]
    public string Title { get; set; }
    
    [BsonElement("comment")]
    public string Comment { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
    
    [BsonElement("ratings")]
    public DetailedRatingsDocument Ratings { get; set; }
    
    [BsonElement("photos")]
    public List<ReviewPhotoDocument> Photos { get; set; } = new();
    
    [BsonElement("helpfulCount")]
    public int HelpfulCount { get; set; }
    
    [BsonElement("helpfulVoters")]
    public List<string> HelpfulVoters { get; set; } = new();
    
    [BsonElement("isVerifiedRental")]
    public bool IsVerifiedRental { get; set; }
    
    [BsonElement("response")]
    public OwnerResponseDocument Response { get; set; }
    
    [BsonElement("status")]
    [BsonRepresentation(BsonType.String)]
    public ModerationStatus Status { get; set; }
    
    [BsonElement("moderationNotes")]
    public string ModerationNotes { get; set; }
}

public class DetailedRatingsDocument
{
    [BsonElement("qualityRating")]
    public int? QualityRating { get; set; }
    
    [BsonElement("accuracyRating")]
    public int? AccuracyRating { get; set; }
    
    [BsonElement("valueRating")]
    public int? ValueRating { get; set; }
    
    [BsonElement("conditionRating")]
    public int? ConditionRating { get; set; }
    
    [BsonElement("communicationRating")]
    public int? CommunicationRating { get; set; }
    
    [BsonElement("reliabilityRating")]
    public int? ReliabilityRating { get; set; }
    
    [BsonElement("flexibilityRating")]
    public int? FlexibilityRating { get; set; }
}

public class OwnerResponseDocument
{
    [BsonElement("comment")]
    public string Comment { get; set; }
    
    [BsonElement("respondedAt")]
    public DateTime RespondedAt { get; set; }
    
    [BsonElement("isEdited")]
    public bool IsEdited { get; set; }
}

public class ReviewPhotoDocument
{
    [BsonElement("url")]
    public string Url { get; set; }
    
    [BsonElement("caption")]
    public string Caption { get; set; }
    
    [BsonElement("order")]
    public int Order { get; set; }
}