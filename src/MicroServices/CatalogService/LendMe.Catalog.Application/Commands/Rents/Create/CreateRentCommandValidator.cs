using FluentValidation;

namespace LendMe.Catalog.Application.Commands.Rents.Create;

public class CreateRentCommandValidator : AbstractValidator<CreateRentCommand>
{
    public CreateRentCommandValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type is required")
            .MaximumLength(50)
            .WithMessage("Type must not exceed 50 characters");

        RuleFor(x => x.ItemIds)
            .NotNull()
            .WithMessage("ItemIds cannot be null")
            .NotEmpty()
            .WithMessage("At least one ItemId is required")
            .Must(ids => ids.All(id => id != Guid.Empty))
            .WithMessage("All ItemIds must be valid GUIDs");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(500)
            .WithMessage("Address must not exceed 500 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.District)
            .NotEmpty()
            .WithMessage("District is required")
            .MaximumLength(100)
            .WithMessage("District must not exceed 100 characters");

        RuleFor(x => x.RadiusMeters)
            .GreaterThan(0)
            .WithMessage("RadiusMeters must be greater than 0")
            .LessThanOrEqualTo(100000)
            .WithMessage("RadiusMeters must not exceed 100000 meters (100 km)");

        RuleFor(x => x.Terms)
            .NotNull().WithMessage("Rental terms are required");

        RuleFor(x => x.Terms.PickupInstructions)
            .NotEmpty()
            .WithMessage("PickupInstructions are required")
            .MaximumLength(1000)
            .WithMessage("PickupInstructions must not exceed 1000 characters");

        RuleFor(x => x.Terms.UsageGuidelines)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrEmpty(x.Terms.UsageGuidelines))
            .WithMessage("UsageGuidelines must not exceed 2000 characters");

        RuleFor(x => x.Terms.CancellationPolicy)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Terms.CancellationPolicy))
            .WithMessage("CancellationPolicy must not exceed 1000 characters");

        RuleFor(x => x.Terms.IncludedAccessories)
            .NotNull()
            .WithMessage("IncludedAccessories cannot be null");

        RuleFor(x => x.Terms.RestrictedUses)
            .NotNull()
            .WithMessage("RestrictedUses cannot be null");

        RuleForEach(x => x.Terms.IncludedAccessories)
            .MaximumLength(200)
            .WithMessage("Each accessory name must not exceed 200 characters");

        RuleForEach(x => x.Terms.RestrictedUses)
            .MaximumLength(200)
            .WithMessage("Each restricted use must not exceed 200 characters");
    }
}