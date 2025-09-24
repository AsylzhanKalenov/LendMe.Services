using Lendme.Core.Entities.Catalog;

namespace Lendme.Core.DapperEntities.Catalog;

public class ItemSearchResult
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
    public string CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
        
    // Геоданные
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public double? DistanceMeters { get; set; }
    public int DeliveryRadiusMeters { get; set; }
    public bool IsInDeliveryRange { get; set; }
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}