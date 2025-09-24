using Lendme.Core.DapperEntities.Catalog;
using Lendme.Core.Entities.Catalog;
using Lendme.Core.Interfaces.Repositories.CatalogRepostories;
using Lendme.Core.Interfaces.Services.CatalogServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Lendme.Infrastructure.Services;

public class ItemSearchService: IItemSearchService
{
    private readonly IItemSearchRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ItemSearchService> _logger;

    public ItemSearchService(
        IItemSearchRepository repository,
        IMemoryCache cache,
        ILogger<ItemSearchService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<PagedResult<ItemSearchResult>> SearchItemsAsync(SearchItemsRequest request)
    {
        try
        {
            // Валидация параметров
            ValidateSearchRequest(request);

            // Формируем ключ кеша
            var cacheKey = GenerateCacheKey(request);

            // Проверяем кеш для GET запросов
            if (request.PageSize <= 20 && _cache.TryGetValue(cacheKey, out PagedResult<ItemSearchResult> cachedResult))
            {
                _logger.LogInformation("Returning cached search results");
                return cachedResult;
            }

            // Выполняем поиск
            var result = await _repository.SearchItemsAsync(request);

            // Сохраняем в кеш на 5 минут
            if (result.Items.Any())
            {
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching items");
            throw;
        }
    }

    public async Task<IEnumerable<ItemSearchResult>> GetNearbyItemsAsync(double latitude, double longitude, int radiusMeters = 5000)
    {
        try
        {
            var cacheKey = $"nearby_{latitude}_{longitude}_{radiusMeters}";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ItemSearchResult> cachedItems))
            {
                return cachedItems;
            }

            var items = await _repository.GetNearbyItemsAsync(latitude, longitude, radiusMeters, 20);
            
            _cache.Set(cacheKey, items, TimeSpan.FromMinutes(10));
            
            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting nearby items");
            throw;
        }
    }

    public async Task<IEnumerable<ItemSearchResult>> GetRecommendedItemsAsync(Guid userId)
    {
        // Здесь можно добавить логику рекомендаций на основе истории просмотров/аренды
        var request = new SearchItemsRequest
        {
            IsAvailable = true,
            Status = ItemStatus.Active,
            PageSize = 10,
            SortBy = SortBy.CreatedAtDesc
        };

        var result = await _repository.SearchItemsAsync(request);
        return result.Items;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        var cacheKey = "all_categories";
        
        if (_cache.TryGetValue(cacheKey, out IEnumerable<Category> cachedCategories))
        {
            return cachedCategories;
        }

        var categories = await _repository.GetCategoriesAsync();
        
        _cache.Set(cacheKey, categories, TimeSpan.FromHours(1));
        
        return categories;
    }

    private void ValidateSearchRequest(SearchItemsRequest request)
    {
        if (request.PageSize > 100)
        {
            throw new ArgumentException("Page size cannot exceed 100 items");
        }

        if (request.PageNumber < 1)
        {
            throw new ArgumentException("Page number must be greater than 0");
        }

        if (request.MinPrice.HasValue && request.MaxPrice.HasValue && request.MinPrice > request.MaxPrice)
        {
            throw new ArgumentException("Minimum price cannot be greater than maximum price");
        }

        if (request.MaxDistanceMeters.HasValue && request.MaxDistanceMeters > 50000)
        {
            throw new ArgumentException("Search radius cannot exceed 50km");
        }
    }

    private string GenerateCacheKey(SearchItemsRequest request)
    {
        return $"search_{request.Latitude}_{request.Longitude}_{request.CategoryId}_{request.MinPrice}_{request.MaxPrice}_{request.PageNumber}_{request.PageSize}_{request.SortBy}";
    }
}