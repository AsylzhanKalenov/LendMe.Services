using LendMe.Catalog.Core.Dto;
using LendMe.Catalog.Core.Entity;

namespace LendMe.Catalog.Core.Interfaces.Repository;

public interface IItemSearchRepository
{
    Task<PagedResult<ItemSearchResult>> SearchItemsAsync(SearchItemsRequest request);
    Task<IEnumerable<ItemSearchResult>> GetNearbyItemsAsync(double latitude, double longitude, int radiusMeters, int limit = 10);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Dictionary<string, int>> GetItemsCountByCityAsync();
}