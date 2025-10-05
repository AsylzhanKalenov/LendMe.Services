using LendMe.Catalog.Application.Dto.Update;
using LendMe.Catalog.Core.Entity;
using LendMe.Catalog.Core.Interfaces.Repository;
using MediatR;

namespace LendMe.Catalog.Application.Commands.Rents.Update;

public sealed record UpdateRentCommand(
    Guid Id,
    string? Title,
    string? Description,
    double? Longitude,
    double? Latitude,
    string? Address,
    string? City,
    string? District,
    int? RadiusMeters,
    RentalTerms? Terms
) : IRequest<UpdateRentResponse>;

public sealed class UpdateRentCommandHandler(IRentRepository repository)
    : IRequestHandler<UpdateRentCommand, UpdateRentResponse>
{
    public async Task<UpdateRentResponse> Handle(UpdateRentCommand request, CancellationToken cancellationToken)
    {
        var rent = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (rent is null)
            throw new KeyNotFoundException($"Rent with id '{request.Id}' not found.");

        rent.UpdateBasicInfo(request.Title, request.Description);
        // TODO: Calculate Min and Max Price later
        //rent.UpdatePriceRange(request.MinPrice, request.MaxPrice);
        rent.UpdateLocation(
            request.Longitude,
            request.Latitude,
            request.Address,
            request.City,
            request.District,
            request.RadiusMeters);
        rent.ReplaceTerms(request.Terms);

        repository.Update(rent);

        var saved = await repository.SaveChangesAsync(cancellationToken);
        if (!saved)
            throw new InvalidOperationException("Failed to save rent changes.");

        return new UpdateRentResponse()
        {
            Id = rent.Id,
            Title = rent.Title,
            Address = rent.Address,
            City = rent.City,
            UpdatedAt = rent.UpdatedAt,
        };
    }
}