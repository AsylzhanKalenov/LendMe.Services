using AutoMapper;
using LendMe.Shared.Application.Reviews.Dto;
using LendMe.Shared.Application.Reviews.Dto.Response;
using LendMe.Shared.Core.Entities.ReviewService;
using LendMe.Shared.Core.Repositories;
using MediatR;

namespace LendMe.Shared.Application.Reviews.Commands.Create;

public class CreateReviewByItemIdCommand : IRequest<CreateReviewResponse>
{
    public ReviewDto CreateReview { get; set; }
    
    public class Handler : IRequestHandler<CreateReviewByItemIdCommand, CreateReviewResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public Handler(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        
        public async Task<CreateReviewResponse> Handle(CreateReviewByItemIdCommand request, CancellationToken cancellationToken)
        {
            var res = await _reviewRepository.CreateReviewAsync(_mapper.Map<Review>(request.CreateReview), cancellationToken) ?? throw new InvalidOperationException();
            return new CreateReviewResponse()
            {
                Id = res.Id
            };
        }
    }
}