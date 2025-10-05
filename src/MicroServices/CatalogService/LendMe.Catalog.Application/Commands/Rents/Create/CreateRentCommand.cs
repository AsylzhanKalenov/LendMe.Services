using LendMe.Catalog.Application.Dto.Create;
using LendMe.Catalog.Core.Interfaces.Repository;
using MediatR;
using NetTopologySuite.Geometries;
using LendMe.Catalog.Core.Entity;

namespace LendMe.Catalog.Application.Commands.Rents.Create;

public class CreateRentCommand : IRequest<CreateRentResponse>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IEnumerable<Guid> ItemIds { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; }
    public CreateRentalTermsDto Terms { get; set; }

    public class Handler : IRequestHandler<CreateRentCommand, CreateRentResponse>
    {
        private readonly IRentRepository _rentRepository;

        public Handler(IRentRepository rentRepository)
        {
            _rentRepository = rentRepository;
        }

        public async Task<CreateRentResponse> Handle(CreateRentCommand request, CancellationToken cancellationToken)
        {
            // Create RentalTerms value object
            var rentalTerms = new RentalTerms(
                request.Terms.PickupInstructions,
                request.Terms.UsageGuidelines,
                request.Terms.IncludedAccessories,
                request.Terms.CancellationPolicy,
                request.Terms.RequiresDeposit,
                request.Terms.RequiresInsurance,
                request.Terms.RestrictedUses);

            // Create Point geometry for PostGIS
            var point = new Point(request.Longitude, request.Latitude) { SRID = 4326 };

            // Create Rent entity
            var rent = new Core.Entity.Rent
            {
                Id = Guid.NewGuid(),
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                Address = request.Address,
                City = request.City,
                District = request.District,
                RadiusMeters = request.RadiusMeters,
                CreatedAt = DateTimeOffset.UtcNow,
                Points = point,
                Terms = rentalTerms
            };

            // Save to database
            await _rentRepository.AddAsync(rent, cancellationToken);
            await _rentRepository.SaveChangesAsync(cancellationToken);

            return new CreateRentResponse
            {
                Id = rent.Id,
                Address = rent.Address,
                City = rent.City,
                CreatedAt = rent.CreatedAt
            };
        }
    }
}