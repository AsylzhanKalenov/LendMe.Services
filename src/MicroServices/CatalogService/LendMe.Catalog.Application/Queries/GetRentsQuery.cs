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
        private readonly IItemSearchService _itemSearchService;
        private readonly IMapper _mapper;

        public Handler(
            IItemSearchService itemSearchService,
            IMapper mapper)
        {
            _itemSearchService = itemSearchService;
            _mapper = mapper;
        }

        public async Task<GetRentsResponse> Handle(GetRentsQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
        
            // Get items with details from PostgreSQL
            var rents = await _itemSearchService.GetNearbyRentsAsync(request.Latitude, request.Longitude);
            //var totalCount = await _itemSearchService.GetTotalCountAsync(cancellationToken);


            return new GetRentsResponse
            {
                Rents = rents,
                //TotalCount = totalCount,
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