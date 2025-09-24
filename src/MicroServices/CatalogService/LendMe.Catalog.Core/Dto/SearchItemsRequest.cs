using LendMe.Catalog.Core.Entity;

namespace LendMe.Catalog.Core.Dto;

public class SearchItemsRequest
{
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? MaxDistanceMeters { get; set; } = 10000; // По умолчанию 10км
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string SearchText { get; set; }
    public bool? IsAvailable { get; set; } = true;
    public ItemStatus? Status { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public SortBy SortBy { get; set; } = SortBy.Distance;
}

public enum SortBy
{
    Distance,
    PriceAsc,
    PriceDesc,
    CreatedAtDesc,
    Title
}