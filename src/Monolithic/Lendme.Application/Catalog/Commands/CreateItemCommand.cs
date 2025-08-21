using AutoMapper;
using Lendme.Core.Entities.Catalog;
using Lendme.Core.Interfaces;
using Lendme.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Lendme.Application.Catalog.Commands;

public class CreateItemCommand : IRequest<CreateItemResponse>
{
    public string Title { get; set; }
    public decimal DailyPrice { get; set; }
    public decimal? WeeklyPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public decimal DepositAmount { get; set; }
    public Guid CategoryId { get; set; }
    public Guid OwnerId { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public List<IFormFile> Images { get; set; }
    public CreateItemLocationDto Location { get; set; }
    public CreateItemRentalTermsDto Terms { get; set; }
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
            var item = new Item(
                request.Title,
                request.DailyPrice,
                request.WeeklyPrice,
                request.MonthlyPrice,
                request.DepositAmount,
                request.CategoryId,
                request.OwnerId);

            // Create Location value object
            var location = new Location(
                request.Location.Coordinates[0],
                request.Location.Coordinates[1],
                request.Location.Address,
                request.Location.City,
                request.Location.District,
                request.Location.RadiusMeters);

            // Create RentalTerms value object
            var rentalTerms = new RentalTerms(
                request.Terms.PickupInstructions,
                request.Terms.UsageGuidelines,
                request.Terms.IncludedAccessories,
                request.Terms.CancellationPolicy,
                request.Terms.RequiresDeposit,
                request.Terms.RequiresInsurance,
                request.Terms.RestrictedUses);

            // Create ItemDetails
            var itemDetails = new ItemDetails(
                item.Id,
                request.Description,
                request.Tags,
                location,
                rentalTerms);

            // Set details to item
            item.SetDetails(itemDetails);

            // Save to PostgreSQL
            await _itemRepository.AddAsync(item, cancellationToken);
            await _itemRepository.SaveChangesAsync(cancellationToken);

            return new CreateItemResponse
            {
                Id = item.Id,
                Title = item.Title,
                CreatedAt = item.CreatedAt
            };
        }
    }
}

public class CreateItemLocationDto
{
    public double[] Coordinates { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; }
}

public class CreateItemRentalTermsDto
{
    public string PickupInstructions { get; set; }
    public string UsageGuidelines { get; set; }
    public List<string> IncludedAccessories { get; set; }
    public string CancellationPolicy { get; set; }
    public bool RequiresDeposit { get; set; }
    public bool RequiresInsurance { get; set; }
    public List<string> RestrictedUses { get; set; }
}

public class CreateItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
}