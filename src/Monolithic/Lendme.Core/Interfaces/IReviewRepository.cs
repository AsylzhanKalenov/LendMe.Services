using Lendme.Core.Entities.ReviewService;

namespace Lendme.Core.Interfaces;

public interface IReviewRepository
{
    Task<List<Review>> GetReviewsByItemAsync(Guid itemId);
    Task<Review?> GetReviewByIdAsync(Guid reviewId);
    Task<Review?> CreateReviewAsync(Review review);
    Task<Review?> UpdateReviewAsync(Review review);
    Task DeleteReviewAsync(Guid reviewId);
}