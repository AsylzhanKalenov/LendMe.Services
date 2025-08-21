using AutoMapper;
using Lendme.Application.Catalog.Queries.Dto;
using Lendme.Core.Interfaces;
using Lendme.Core.Interfaces.Repositories;
using MediatR;

namespace Lendme.Application.Catalog.Queries;

public class GetItemsQuery : IRequest<GetItemsResponse>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    
    public class Handler : IRequestHandler<GetItemsQuery, GetItemsResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public Handler(
            IItemRepository itemRepository,
            IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<GetItemsResponse> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
        
            // Get items with details from PostgreSQL
            var items = await _itemRepository.GetAllWithDetailsAsync(skip, request.PageSize, cancellationToken);
            var totalCount = await _itemRepository.GetTotalCountAsync(cancellationToken);

            // Map to DTOs
            var itemDtos = _mapper.Map<List<ItemDto>>(items);

            return new GetItemsResponse
            {
                Items = itemDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}

public class GetItemsResponse
{
    public List<ItemDto> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}