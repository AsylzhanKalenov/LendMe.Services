namespace LendMe.Shared.Application.Reviews.Dto;

// Read DTO
public sealed class ReviewDto
{
    public Guid Id { get; init; }
    public Guid BookingId { get; init; }
    public Guid ItemId { get; init; }
    public Guid ReviewerId { get; init; }
    public Guid RevieweeId { get; init; }
    public string Type { get; init; } // строкой для стабильного контракта
    public int Rating { get; init; }
    public string Title { get; init; }
    public string Comment { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public int HelpfulCount { get; init; }
    public bool IsVerifiedRental { get; init; }
    public string Status { get; init; }
    // Урезаем тяжелые вложенности до потребностей UI
    public IReadOnlyList<ReviewPhotoDto> Photos { get; init; }
    public OwnerResponseDto Response { get; init; }
}

public sealed class ReviewPhotoDto
{
    public string Url { get; init; }
    public string Caption { get; init; }
    public int Order { get; init; }
}

public sealed class OwnerResponseDto
{
    public string Comment { get; init; }
    public DateTime RespondedAt { get; init; }
    public bool IsEdited { get; init; }
}

// Create/Update DTO
public sealed class ReviewCreateDto
{
    public Guid BookingId { get; init; }
    public Guid ItemId { get; init; }
    public Guid ReviewerId { get; init; }
    public Guid RevieweeId { get; init; }
    public string Type { get; init; } // "ItemReview"/"RenterReview"/"OwnerReview"
    public int Rating { get; init; }
    public string Title { get; init; }
    public string Comment { get; init; }
    public IReadOnlyList<ReviewPhotoDto> Photos { get; init; }
}