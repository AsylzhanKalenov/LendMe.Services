using LendMe.Catalog.Core.Dto;

namespace LendMe.Catalog.Core.Interfaces.Services;

public interface IRentSearchService
{
    
    Task<IEnumerable<RentSearchResult>> GetNearbyRentsAsync(double latitude, double longitude, int radiusMeters = 5000);
}