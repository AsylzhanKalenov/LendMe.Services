namespace LendMe.Catalog.Core.Entity;

public class Item
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string IdentifyNumber { get; set; }
    public PriceType PriceType { get; set; } = PriceType.Daily;
    public decimal? DailyPrice { get; set; }
    public decimal? HourlyPrice { get; set; }
    public decimal? WeeklyPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public decimal? DepositAmount { get; set; }
    public bool IsAvailable { get; set; }
    public ItemStatus Status { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public Guid OwnerId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    
    // Navigation property
    public ItemDetails Details { get; private set; }
    
    public Item(
        string title,
        string identifyNumber,
        PriceType priceType,
        decimal? dailyPrice,
        decimal? hourlyPrice,
        decimal? weeklyPrice,
        decimal? monthlyPrice,
        decimal? depositAmount,
        Guid categoryId,
        Guid ownerId)
    {
        Id = Guid.NewGuid();
        Title = title;
        IdentifyNumber = identifyNumber;
        PriceType = priceType;
        DailyPrice = dailyPrice;
        HourlyPrice = hourlyPrice;
        WeeklyPrice = weeklyPrice;
        MonthlyPrice = monthlyPrice;
        DepositAmount = depositAmount;
        IsAvailable = true;
        Status = ItemStatus.Draft;
        CategoryId = categoryId;
        OwnerId = ownerId;
        CreatedAt = DateTimeOffset.UtcNow;
        IsDeleted = false;
    }
    
    public void SetDetails(ItemDetails details)
    {
        Details = details;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
        UpdatedAt = DateTimeOffset.UtcNow; 
    }

    public void UpdateStatus(ItemStatus status)
    {
        Status = status;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    
    public void UpdateMainInfo(string? title)
    {
        EnsureNotDeleted();
        var changed = false;

        if (title is not null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            if (!string.Equals(Title, title, StringComparison.Ordinal))
            {
                Title = title;
                changed = true;
            }
        }

        if (changed) Touch();
    }

    public void UpdatePricing(
        PriceType? priceType,
        decimal? dailyPrice,
        decimal? hourlyPrice,
        decimal? weeklyPrice,
        decimal? monthlyPrice,
        decimal? depositAmount)
    {
        EnsureNotDeleted();
        var changed = false;

        // Validate incoming values (if provided)
        ValidateNonNegative(depositAmount, nameof(depositAmount));
        ValidatePositiveIfProvided(dailyPrice, nameof(dailyPrice));
        ValidatePositiveIfProvided(hourlyPrice, nameof(hourlyPrice));
        ValidatePositiveIfProvided(weeklyPrice, nameof(weeklyPrice));
        ValidatePositiveIfProvided(monthlyPrice, nameof(monthlyPrice));

        // Apply partial updates
        if (priceType.HasValue && PriceType != priceType.Value)
        {
            PriceType = priceType.Value;
            changed = true;
        }

        if (dailyPrice.HasValue && DailyPrice != dailyPrice)
        {
            DailyPrice = dailyPrice;
            changed = true;
        }
        if (hourlyPrice.HasValue && HourlyPrice != hourlyPrice)
        {
            HourlyPrice = hourlyPrice;
            changed = true;
        }
        if (weeklyPrice.HasValue && WeeklyPrice != weeklyPrice)
        {
            WeeklyPrice = weeklyPrice;
            changed = true;
        }
        if (monthlyPrice.HasValue && MonthlyPrice != monthlyPrice)
        {
            MonthlyPrice = monthlyPrice;
            changed = true;
        }
        if (depositAmount.HasValue && DepositAmount != depositAmount)
        {
            DepositAmount = depositAmount;
            changed = true;
        }

        // Validate selected price type has an effective value (> 0)
        var effectiveType = priceType ?? PriceType;

        var effectiveDaily = dailyPrice ?? DailyPrice;
        var effectiveHourly = hourlyPrice ?? HourlyPrice;
        var effectiveWeekly = weeklyPrice ?? WeeklyPrice;
        var effectiveMonthly = monthlyPrice ?? MonthlyPrice;

        var selectedPrice = effectiveType switch
        {
            PriceType.Daily => effectiveDaily,
            PriceType.Hourly => effectiveHourly,
            PriceType.Weekly => effectiveWeekly,
            PriceType.Monthly => effectiveMonthly,
            _ => null
        };

        if (selectedPrice is null || selectedPrice <= 0)
            throw new InvalidOperationException($"Price for {effectiveType} must be provided and greater than zero.");

        if (changed) Touch();
    }

    // Optionally, add a method for details if ваша модель Details это поддерживает:
    // public void UpdateDetails(string? description, IEnumerable<string>? tags) { ... }

    // ----------------- Helpers -----------------

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot modify a deleted item.");
    }

    private static void ValidatePositiveIfProvided(decimal? value, string paramName)
    {
        if (value.HasValue && value.Value <= 0)
            throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.");
    }

    private static void ValidateNonNegative(decimal? value, string paramName)
    {
        if (value.HasValue && value.Value < 0)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
    }

    private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;

    
}

// Enums
public enum ItemStatus { Draft, Active, Inactive, Blocked, UnderReview }
public enum PriceType { Hourly, Daily, Weekly, Monthly }