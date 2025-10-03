using LendMe.Catalog.Core.Dto;

namespace LendMe.Catalog.Core.Interfaces.Repository;

public interface IRentSearchRepository
{
    Task<PagedResult<RentSearchResult>> SearchRentsAsync(SearchRentsRequest request);
    Task<IEnumerable<RentSearchResult>> GetNearbyRentsAsync(double latitude, double longitude, int radiusMeters, int limit = 10);
}