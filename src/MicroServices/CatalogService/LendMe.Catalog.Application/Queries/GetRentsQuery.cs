using AutoMapper;
using LendMe.Catalog.Core.Dto;
using LendMe.Catalog.Core.Interfaces.Services;
using MediatR;

namespace LendMe.Catalog.Application.Queries;

public class GetRentsQuery : IRequest<GetRentsResponse>
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    
    public class Handler : IRequestHandler<GetRentsQuery, GetRentsResponse>
    {
        private readonly IRentSearchService _rentSearchService;
        private readonly IMapper _mapper;

        public Handler(
            IRentSearchService rentSearchService,
            IMapper mapper)
        {
            _rentSearchService = rentSearchService;
            _mapper = mapper;
        }

        public async Task<GetRentsResponse> Handle(GetRentsQuery request, CancellationToken cancellationToken)
        {
            var rents = await _rentSearchService.GetNearbyRentsAsync(request.Latitude, request.Longitude);
         
            return new GetRentsResponse
            {
                Rents = rents,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}

public class GetRentsResponse
{
    public IEnumerable<RentSearchResult> Rents { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}