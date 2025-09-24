namespace LendMe.Catalog.Core.Entity;

public class Item
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal DailyPrice { get; set; }
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
        decimal dailyPrice,
        decimal? weeklyPrice,
        decimal? monthlyPrice,
        decimal? depositAmount,
        Guid categoryId,
        Guid ownerId)
    {
        Id = Guid.NewGuid();
        Title = title;
        DailyPrice = dailyPrice;
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
}

// Enums
public enum ItemStatus { Draft, Active, Inactive, Blocked, UnderReview }