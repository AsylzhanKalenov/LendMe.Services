using LendMe.Shared.Core.Entities.ReviewService;
using LendMe.Shared.Infrastructure.MongoPersistence.Documents.ReviewDocs;

namespace LendMe.Shared.Infrastructure.MongoPersistence.Mapper;

public static class ReviewMapper
{
    public static ReviewDocument ToDocument(this Review review)
    {
        if (review == null) return null;

        return new ReviewDocument
        {
            Id = review.Id,
            BookingId = review.BookingId,
            ItemId = review.ItemId,
            ReviewerId = review.ReviewerId,
            RevieweeId = review.RevieweeId,
            Type = review.Type,
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt,
            Ratings = review.Ratings?.ToDocument(),
            Photos = review.Photos?.Select(p => p.ToDocument()).ToList() ?? new List<ReviewPhotoDocument>(),
            HelpfulCount = review.HelpfulCount,
            HelpfulVoters = review.HelpfulVoters ?? new List<string>(),
            IsVerifiedRental = review.IsVerifiedRental,
            Response = review.Response?.ToDocument(),
            Status = review.Status,
            ModerationNotes = review.ModerationNotes
        };
    }

    public static Review? ToEntity(this ReviewDocument? document)
    {
        if (document == null) return null;

        return new Review
        {
            Id = document.Id,
            BookingId = document.BookingId,
            ItemId = document.ItemId,
            ReviewerId = document.ReviewerId,
            RevieweeId = document.RevieweeId,
            Type = document.Type,
            Rating = document.Rating,
            Title = document.Title,
            Comment = document.Comment,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt,
            Ratings = document.Ratings?.ToEntity(),
            Photos = document.Photos?.Select(p => p.ToEntity()).ToList() ?? new List<ReviewPhoto>(),
            HelpfulCount = document.HelpfulCount,
            HelpfulVoters = document.HelpfulVoters ?? new List<string>(),
            IsVerifiedRental = document.IsVerifiedRental,
            Response = document.Response?.ToEntity(),
            Status = document.Status,
            ModerationNotes = document.ModerationNotes
        };
    }

    public static List<Review> ToEntityList(this IEnumerable<ReviewDocument> documents)
    {
        return documents?.Select(d => d.ToEntity()).ToList() ?? new List<Review>();
    }

    public static List<ReviewDocument> ToDocumentList(this IEnumerable<Review> reviews)
    {
        return reviews?.Select(r => r.ToDocument()).ToList() ?? new List<ReviewDocument>();
    }

    private static DetailedRatingsDocument ToDocument(this DetailedRatings ratings)
    {
        if (ratings == null) return null;

        return new DetailedRatingsDocument
        {
            QualityRating = ratings.QualityRating,
            AccuracyRating = ratings.AccuracyRating,
            ValueRating = ratings.ValueRating,
            ConditionRating = ratings.ConditionRating,
            CommunicationRating = ratings.CommunicationRating,
            ReliabilityRating = ratings.ReliabilityRating,
            FlexibilityRating = ratings.FlexibilityRating
        };
    }

    private static DetailedRatings ToEntity(this DetailedRatingsDocument document)
    {
        if (document == null) return null;

        return new DetailedRatings
        {
            QualityRating = document.QualityRating,
            AccuracyRating = document.AccuracyRating,
            ValueRating = document.ValueRating,
            ConditionRating = document.ConditionRating,
            CommunicationRating = document.CommunicationRating,
            ReliabilityRating = document.ReliabilityRating,
            FlexibilityRating = document.FlexibilityRating
        };
    }

    private static OwnerResponseDocument ToDocument(this OwnerResponse response)
    {
        if (response == null) return null;

        return new OwnerResponseDocument
        {
            Comment = response.Comment,
            RespondedAt = response.RespondedAt,
            IsEdited = response.IsEdited
        };
    }

    private static OwnerResponse ToEntity(this OwnerResponseDocument document)
    {
        if (document == null) return null;

        return new OwnerResponse
        {
            Comment = document.Comment,
            RespondedAt = document.RespondedAt,
            IsEdited = document.IsEdited
        };
    }

    private static ReviewPhotoDocument ToDocument(this ReviewPhoto photo)
    {
        if (photo == null) return null;

        return new ReviewPhotoDocument
        {
            Url = photo.Url,
            Caption = photo.Caption,
            Order = photo.Order
        };
    }

    private static ReviewPhoto ToEntity(this ReviewPhotoDocument document)
    {
        if (document == null) return null;

        return new ReviewPhoto
        {
            Url = document.Url,
            Caption = document.Caption,
            Order = document.Order
        };
    }
}