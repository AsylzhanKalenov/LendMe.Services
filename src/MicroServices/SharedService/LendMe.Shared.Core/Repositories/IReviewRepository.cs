using LendMe.Shared.Core.Entities.ReviewService;

namespace LendMe.Shared.Core.Repositories;

public interface IReviewRepository
{
    Task<List<Review>> GetReviewsByItemAsync(string itemId);
    Task<Review?> GetReviewByIdAsync(Guid reviewId);
    Task<Review?> CreateReviewAsync(Review review);
    Task<Review?> UpdateReviewAsync(Review review);
    Task DeleteReviewAsync(Guid reviewId);
}