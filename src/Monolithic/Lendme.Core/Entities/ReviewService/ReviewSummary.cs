namespace Lendme.Core.Entities.ReviewService;

public class ReviewSummary
{
    //Mongo
    //public ObjectId Id { get; set; }
    public Guid Id { get; set; }
    public Guid EntityId { get; set; } // ItemId or UserId
    public EntityType EntityType { get; set; }
    
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public Dictionary<int, int> RatingDistribution { get; set; } // 5:50, 4:30, etc.
    
    // Aggregated detailed ratings
    public Dictionary<string, double> DetailedAverages { get; set; }
    
    // Keywords from AI analysis
    public List<ReviewKeyword> PositiveKeywords { get; set; }
    public List<ReviewKeyword> NegativeKeywords { get; set; }
    
    public DateTime LastUpdated { get; set; }
}

public class ReviewKeyword
{
    public string Keyword { get; set; }
    public int Count { get; set; }
    public double Sentiment { get; set; }
}