using AutoMapper;
using LendMe.Catalog.Core.Dto;
using LendMe.Catalog.Core.Interfaces.Services;
using MediatR;

namespace LendMe.Catalog.Application.Queries;

public class GetItemsQuery : IRequest<GetItemsResponse>
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    
    public class Handler : IRequestHandler<GetItemsQuery, GetItemsResponse>
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

        public async Task<GetItemsResponse> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
        
            // Get items with details from PostgreSQL
            var items = await _itemSearchService.GetNearbyItemsAsync(request.Latitude, request.Longitude);
            //var totalCount = await _itemSearchService.GetTotalCountAsync(cancellationToken);


            return new GetItemsResponse
            {
                Items = items,
                //TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}

public class GetItemsResponse
{
    public IEnumerable<ItemSearchResult> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}