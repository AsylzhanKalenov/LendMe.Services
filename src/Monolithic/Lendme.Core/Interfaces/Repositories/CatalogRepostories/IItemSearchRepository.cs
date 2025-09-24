using Lendme.Core.DapperEntities.Catalog;
using Lendme.Core.Entities.Catalog;

namespace Lendme.Core.Interfaces.Repositories.CatalogRepostories;

public interface IItemSearchRepository
{
    Task<PagedResult<ItemSearchResult>> SearchItemsAsync(SearchItemsRequest request);
    Task<IEnumerable<ItemSearchResult>> GetNearbyItemsAsync(double latitude, double longitude, int radiusMeters, int limit = 10);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Dictionary<string, int>> GetItemsCountByCityAsync();
}