using LendMe.Shared.Core.Entities.ReviewService;

namespace LendMe.Shared.Application.Reviews.Dto.Response;

public class GetItemReviewsResponse
{
    public List<Review> Reviews {get; set;}
}