using LendMe.Catalog.Core.Dto;
using LendMe.Catalog.Core.Entity;

namespace LendMe.Catalog.Core.Interfaces.Services;

public interface IItemSearchService
{
    Task<PagedResult<ItemSearchResult>> SearchItemsAsync(SearchItemsRequest request);
    Task<IEnumerable<ItemSearchResult>> GetNearbyItemsAsync(double latitude, double longitude, int radiusMeters = 5000);
    Task<IEnumerable<ItemSearchResult>> GetRecommendedItemsAsync(Guid userId);
    Task<IEnumerable<Category>> GetCategoriesAsync();
}