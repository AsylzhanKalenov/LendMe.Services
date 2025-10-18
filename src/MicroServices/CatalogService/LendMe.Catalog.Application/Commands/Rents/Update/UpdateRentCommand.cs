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
    UpdateRentalTermsDto? Terms
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
        rent.ReplaceTerms(MapTerms(request.Terms));

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
    
    private static RentalTerms MapTerms(UpdateRentalTermsDto? dto, RentalTerms? existing = null)
    {
        if (dto is null)
            return existing ?? new RentalTerms(
                pickupInstructions: string.Empty,
                usageGuidelines: null,
                includedAccessories: new List<string>(),
                cancellationPolicy: null,
                requiresDeposit: false,
                requiresInsurance: false,
                restrictedUses: new List<string>());

        // Если есть существующий объект, обновляем его поля.
        var target = existing ?? new RentalTerms(
            pickupInstructions: dto.PickupInstructions ?? string.Empty,
            usageGuidelines: dto.UsageGuidelines,
            includedAccessories: dto.IncludedAccessories ?? new List<string>(),
            cancellationPolicy: dto.CancellationPolicy,
            requiresDeposit: dto.RequiresDeposit,
            requiresInsurance: dto.RequiresInsurance,
            restrictedUses: dto.RestrictedUses ?? new List<string>());

        // Присваивания (обновление значений)
        target.PickupInstructions = dto.PickupInstructions ?? string.Empty;
        target.UsageGuidelines = dto.UsageGuidelines;
        target.IncludedAccessories = (dto.IncludedAccessories ?? new List<string>()).ToList();
        target.CancellationPolicy = dto.CancellationPolicy;
        target.RequiresDeposit = dto.RequiresDeposit;
        target.RequiresInsurance = dto.RequiresInsurance;
        target.RestrictedUses = (dto.RestrictedUses ?? new List<string>()).ToList();

        return target;
    }
}