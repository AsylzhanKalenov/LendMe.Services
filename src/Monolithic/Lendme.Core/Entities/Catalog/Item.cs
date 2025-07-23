namespace Lendme.Core.Entities.Catalog;

public class Item
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal DailyPrice { get; set; }
    public decimal? WeeklyPrice { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public decimal DepositAmount { get; set; }
    public bool IsAvailable { get; set; }
    public ItemStatus Status { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

// Enums
public enum ItemStatus { Draft, Active, Inactive, Blocked, UnderReview }