using Lendme.Core.DapperEntities.Catalog;
using Lendme.Core.Entities.Catalog;

namespace Lendme.Core.Interfaces.Services.CatalogServices;

public interface IItemSearchService
{
    Task<PagedResult<ItemSearchResult>> SearchItemsAsync(SearchItemsRequest request);
    Task<IEnumerable<ItemSearchResult>> GetNearbyItemsAsync(double latitude, double longitude, int radiusMeters = 5000);
    Task<IEnumerable<ItemSearchResult>> GetRecommendedItemsAsync(Guid userId);
    Task<IEnumerable<Category>> GetCategoriesAsync();
}