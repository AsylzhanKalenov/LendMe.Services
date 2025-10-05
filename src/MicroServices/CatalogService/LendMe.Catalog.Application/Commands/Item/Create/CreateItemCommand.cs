using AutoMapper;
using LendMe.Catalog.Application.Dto.Create;
using LendMe.Catalog.Core.Entity;
using LendMe.Catalog.Core.Interfaces.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LendMe.Catalog.Application.Commands.Item.Create;

public class CreateItemCommand : IRequest<CreateItemResponse>
{
    public string Title { get; set; }
    public string IdentifyNumber { get; set; }
    public PriceType PriceType { get; set; } = PriceType.Daily;
    public decimal? DailyPrice { get; set; }
    public decimal? HourlyPrice { get; set; }
    public decimal? WeeklyPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public decimal? DepositAmount { get; set; }
    public Guid CategoryId { get; set; }
    public Guid OwnerId { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public List<IFormFile>? Images { get; set; }
    public class Handler : IRequestHandler<CreateItemCommand, CreateItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public Handler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<CreateItemResponse> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            // Create Item entity
            var item = new Core.Entity.Item(
                request.Title,
                request.IdentifyNumber,
                request.PriceType,
                request.DailyPrice,
                request.HourlyPrice,
                request.WeeklyPrice,
                request.MonthlyPrice,
                request.DepositAmount,
                request.CategoryId,
                request.OwnerId);

            // Create ItemDetails
            var itemDetails = new ItemDetails(
                item.Id,
                request.Description,
                request.Tags);

            // Set details to item
            item.SetDetails(itemDetails);

            // Save to PostgreSQL
            await _itemRepository.AddAsync(item, cancellationToken);
            await _itemRepository.SaveChangesAsync(cancellationToken);

            return new CreateItemResponse
            {
                Id = item.Id,
                IdentifyNumber = item.IdentifyNumber,
                Title = item.Title,
                UpdatedAt = item.CreatedAt
            };
        }
    }
}