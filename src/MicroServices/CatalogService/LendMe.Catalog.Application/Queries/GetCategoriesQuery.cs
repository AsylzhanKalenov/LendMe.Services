using AutoMapper;
using LendMe.Catalog.Application.Dto;
using LendMe.Catalog.Core.Interfaces.Services;
using MediatR;

namespace LendMe.Catalog.Application.Queries;

public class GetCategoriesQuery : IRequest<List<CategoryDto>>
{
    public class Handler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IItemSearchService _itemSearchService;
        private readonly IMapper _mapper;

        public Handler(IItemSearchService itemSearchService, IMapper mapper)
        {
            _itemSearchService = itemSearchService;
            _mapper = mapper;
        }
        
        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _itemSearchService.GetCategoriesAsync();
            
            return _mapper.Map<List<CategoryDto>>(categories);
        }
    }
}