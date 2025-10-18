using LendMe.Catalog.Core.Dto;
using LendMe.Catalog.Core.Interfaces.Repository;
using LendMe.Catalog.Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LendMe.Catalog.Infrastructure.Services;

public class RentSearchService : IRentSearchService
{
    private readonly IRentSearchRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<RentSearchService> _logger;

    public RentSearchService(
        IRentSearchRepository repository,
        IMemoryCache cache,
        ILogger<RentSearchService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<IEnumerable<RentSearchResult>> GetNearbyRentsAsync(double latitude, double longitude, int radiusMeters = 5000)
    {
        try
        {
            var cacheKey = $"nearby_{latitude}_{longitude}_{radiusMeters}";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<RentSearchResult> cachedItems))
            {
                return cachedItems;
            }

            var items = await _repository.GetNearbyRentsAsync(latitude, longitude, radiusMeters, 20);
            
            _cache.Set(cacheKey, items, TimeSpan.FromMinutes(10));
            
            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting nearby items");
            throw;
        }
    }
}