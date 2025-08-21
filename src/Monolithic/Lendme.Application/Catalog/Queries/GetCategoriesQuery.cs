using AutoMapper;
using Lendme.Application.Catalog.Queries.Dto;
using Lendme.Core.Interfaces;
using Lendme.Core.Interfaces.Repositories;
using MediatR;

namespace Lendme.Application.Catalog.Queries;

public abstract class GetCategoriesQuery : IRequest<List<CategoryDto>>
{
    
    public class Handler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public Handler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        
        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetCategoriesAsync(cancellationToken);
            
            return _mapper.Map<List<CategoryDto>>(categories);
        }
    }
}