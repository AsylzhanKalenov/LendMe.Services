using LendMe.Catalog.Application.Dto.Update;
using LendMe.Catalog.Core.Entity;
using LendMe.Catalog.Core.Interfaces.Repository;
using MediatR;

namespace LendMe.Catalog.Application.Commands.Item.Update;

public sealed record UpdateItemCommand(
    Guid Id,
    string? Title,
    PriceType? PriceType,
    decimal? DailyPrice,
    decimal? HourlyPrice,
    decimal? WeeklyPrice,
    decimal? MonthlyPrice,
    decimal? DepositAmount,
    bool? IsAvailable,
    ItemStatus? Status
) : IRequest<UpdateItemResponse>;

public sealed class UpdateItemCommandHandler(IItemRepository repository)
    : IRequestHandler<UpdateItemCommand, UpdateItemResponse>
{
    public async Task<UpdateItemResponse> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        // 1) Загрузить агрегат
        var item = await repository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (item is null)
            throw new KeyNotFoundException($"Item with id '{request.Id}' not found.");

        // 2) Применить бизнес-изменения через доменные методы
        item.UpdateMainInfo(request.Title);

        item.UpdatePricing(
            request.PriceType,
            request.DailyPrice,
            request.HourlyPrice,
            request.WeeklyPrice,
            request.MonthlyPrice,
            request.DepositAmount
        );

        if (request.IsAvailable.HasValue)
            item.UpdateAvailability(request.IsAvailable.Value);

        if (request.Status.HasValue)
            item.UpdateStatus(request.Status.Value);

        // 3) Зафиксировать изменения
        // Если ваш репозиторий/ORM отслеживает сущности после GetById..., вызов Update может быть не нужен.
        repository.Update(item);

        var saved = await repository.SaveChangesAsync(cancellationToken);
        if (!saved)
            throw new InvalidOperationException("Failed to save item changes.");

        return new UpdateItemResponse()
        {
            Id = item.Id,
            IdentifyNumber = item.IdentifyNumber,
            Title = item.Title,
            CreatedAt = item.CreatedAt
        };
    }
}