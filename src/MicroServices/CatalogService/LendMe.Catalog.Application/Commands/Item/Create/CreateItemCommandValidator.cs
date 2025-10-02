using FluentValidation;

namespace LendMe.Catalog.Application.Commands.Item.Create;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.DailyPrice)
            .GreaterThan(0).WithMessage("Daily price must be greater than 0");

        RuleFor(x => x.WeeklyPrice)
            .GreaterThan(0).When(x => x.WeeklyPrice.HasValue)
            .WithMessage("Weekly price must be greater than 0");

        RuleFor(x => x.MonthlyPrice)
            .GreaterThan(0).When(x => x.MonthlyPrice.HasValue)
            .WithMessage("Monthly price must be greater than 0");

        RuleFor(x => x.DepositAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Deposit amount must be non-negative");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");
    }
}