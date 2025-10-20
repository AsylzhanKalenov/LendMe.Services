using LendMe.Shared.Application.Reviews.Dto.Response;
using LendMe.Shared.Core.Repositories;
using MediatR;

namespace LendMe.Shared.Application.Reviews.Queries;

public class GetItemReviewsQuery : IRequest<GetItemReviewsResponse>
{
    public string ItemId { get; set; }
    
    public class Handler : IRequestHandler<GetItemReviewsQuery, GetItemReviewsResponse>
    {
        private readonly IReviewRepository _reviewRepository;

        public Handler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        
        public async Task<GetItemReviewsResponse> Handle(GetItemReviewsQuery request, CancellationToken cancellationToken)
        {
            return new GetItemReviewsResponse()
            {
                Reviews = await _reviewRepository.GetReviewsByItemAsync(request.ItemId, cancellationToken)
            };
        }
    }
}