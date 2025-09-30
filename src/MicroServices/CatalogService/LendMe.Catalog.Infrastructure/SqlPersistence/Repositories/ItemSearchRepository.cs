using System.Text;
using Dapper;
using LendMe.Catalog.Core.Dto;
using LendMe.Catalog.Core.Entity;
using LendMe.Catalog.Core.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace LendMe.Catalog.Infrastructure.SqlPersistence.Repositories;

public class ItemSearchRepository: IItemSearchRepository
{
    private readonly string _connectionString;

    public ItemSearchRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<PagedResult<ItemSearchResult>> SearchItemsAsync(SearchItemsRequest request)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        
        var sqlBuilder = new StringBuilder();
        var parameters = new DynamicParameters();
        
        // Основной запрос с JOIN-ами
        sqlBuilder.Append(@"
            WITH search_results AS (
                SELECT DISTINCT
                    i.id,
                    i.title,
                    i.daily_price,
                    i.weekly_price,
                    i.monthly_price,
                    i.deposit_amount,
                    i.is_available,
                    i.status,
                    i.category_id,
                    c.name as category_name,
                    i.created_at,
                    r.latitude,
                    r.longitude,
                    r.address,
                    r.city,
                    r.district,
                    r.radius_meters as delivery_radius_meters");

        // Добавляем расчет расстояния если есть координаты пользователя
        if (request.Latitude.HasValue && request.Longitude.HasValue)
        {
            sqlBuilder.Append(@",
                ST_Distance(
                    r.location::geography,
                    ST_SetSRID(ST_MakePoint(@UserLongitude, @UserLatitude), 4326)::geography
                ) as distance_meters,
                CASE 
                    WHEN ST_Distance(
                        r.location::geography,
                        ST_SetSRID(ST_MakePoint(@UserLongitude, @UserLatitude), 4326)::geography
                    ) <= r.radius_meters THEN true
                    ELSE false
                END as is_in_delivery_range");
            
            parameters.Add("UserLatitude", request.Latitude.Value);
            parameters.Add("UserLongitude", request.Longitude.Value);
        }
        else
        {
            sqlBuilder.Append(@",
                NULL as distance_meters,
                false as is_in_delivery_range");
        }

        sqlBuilder.Append(@"
            FROM items i
            INNER JOIN rent_items ri ON ri.item_id = i.id
            INNER JOIN rents r ON r.id = ri.rent_id
            LEFT JOIN categories c ON c.id = i.category_id
            WHERE i.is_deleted = false");

        // Добавляем фильтры
        BuildWhereClause(sqlBuilder, parameters, request);

        sqlBuilder.Append(@"
            )
            SELECT * FROM search_results");

        // Добавляем сортировку
        AddSorting(sqlBuilder, request);

        // Добавляем пагинацию
        sqlBuilder.Append(@"
            LIMIT @PageSize OFFSET @Offset");
        
        parameters.Add("PageSize", request.PageSize);
        parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);

        // Получаем данные
        var items = await connection.QueryAsync<ItemSearchResult>(sqlBuilder.ToString(), parameters);

        // Получаем общее количество записей для пагинации
        var countQuery = BuildCountQuery(request);
        var totalCount = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

        return new PagedResult<ItemSearchResult>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    private void BuildWhereClause(StringBuilder sqlBuilder, DynamicParameters parameters, SearchItemsRequest request)
    {
        // Фильтр по расстоянию
        if (request.Latitude.HasValue && request.Longitude.HasValue && request.MaxDistanceMeters.HasValue)
        {
            sqlBuilder.Append(@"
                AND ST_DWithin(
                    r.location::geography,
                    ST_SetSRID(ST_MakePoint(@UserLongitude, @UserLatitude), 4326)::geography,
                    @MaxDistance
                )");
            parameters.Add("MaxDistance", request.MaxDistanceMeters.Value);
        }

        // Фильтр по категории
        if (request.CategoryId.HasValue)
        {
            sqlBuilder.Append(" AND i.category_id = @CategoryId");
            parameters.Add("CategoryId", request.CategoryId.Value);
        }

        // Фильтр по цене
        if (request.MinPrice.HasValue)
        {
            sqlBuilder.Append(" AND i.daily_price >= @MinPrice");
            parameters.Add("MinPrice", request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            sqlBuilder.Append(" AND i.daily_price <= @MaxPrice");
            parameters.Add("MaxPrice", request.MaxPrice.Value);
        }

        // Поиск по тексту
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            sqlBuilder.Append(" AND i.title ILIKE @SearchText");
            parameters.Add("SearchText", $"%{request.SearchText}%");
        }

        // Фильтр по доступности
        if (request.IsAvailable.HasValue)
        {
            sqlBuilder.Append(" AND i.is_available = @IsAvailable");
            parameters.Add("IsAvailable", request.IsAvailable.Value);
        }

        // Фильтр по статусу
        if (request.Status.HasValue)
        {
            sqlBuilder.Append(" AND i.status = @Status");
            parameters.Add("Status", (int)request.Status.Value);
        }

        // Фильтр по городу
        if (!string.IsNullOrWhiteSpace(request.City))
        {
            sqlBuilder.Append(" AND r.city ILIKE @City");
            parameters.Add("City", $"%{request.City}%");
        }

        // Фильтр по району
        if (!string.IsNullOrWhiteSpace(request.District))
        {
            sqlBuilder.Append(" AND r.district ILIKE @District");
            parameters.Add("District", $"%{request.District}%");
        }
    }

    private void AddSorting(StringBuilder sqlBuilder, SearchItemsRequest request)
    {
        sqlBuilder.Append(" ORDER BY ");
        
        switch (request.SortBy)
        {
            case SortBy.Distance:
                if (request.Latitude.HasValue && request.Longitude.HasValue)
                {
                    sqlBuilder.Append("distance_meters ASC NULLS LAST");
                }
                else
                {
                    sqlBuilder.Append("created_at DESC");
                }
                break;
            case SortBy.PriceAsc:
                sqlBuilder.Append("daily_price ASC");
                break;
            case SortBy.PriceDesc:
                sqlBuilder.Append("daily_price DESC");
                break;
            case SortBy.CreatedAtDesc:
                sqlBuilder.Append("created_at DESC");
                break;
            case SortBy.Title:
                sqlBuilder.Append("title ASC");
                break;
            default:
                sqlBuilder.Append("created_at DESC");
                break;
        }
    }

    private string BuildCountQuery(SearchItemsRequest request)
    {
        var sqlBuilder = new StringBuilder(@"
            SELECT COUNT(DISTINCT i.id)
            FROM items i
            INNER JOIN rent_items ri ON ri.item_id = i.id
            INNER JOIN rents r ON r.id = ri.rent_id
            WHERE i.is_deleted = false");

        // Используем те же фильтры, но без параметров (они уже добавлены)
        var whereClause = new StringBuilder();
        BuildWhereClause(whereClause, new DynamicParameters(), request);
        
        return sqlBuilder.ToString();
    }

    public async Task<IEnumerable<ItemSearchResult>> GetNearbyItemsAsync(double latitude, double longitude, int radiusMeters, int limit = 10)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        
        var query = @"
            SELECT 
                i.Id,
                i.Title,
                i.DailyPrice,
                i.WeeklyPrice,
                i.MonthlyPrice,
                i.IsAvailable,
                i.Status,
                c.Name as CategoryName,
                r.Latitude,
                r.Longitude,
                r.Address,
                r.City,
                r.District,
                ST_Distance(
                    r.location::geography,
                    ST_SetSRID(ST_MakePoint(@Longitude, @Latitude), 4326)::geography
                ) as distance_meters
            FROM Items i
            INNER JOIN RentItems ri ON ri.ItemId = i.Id
            INNER JOIN Rent r ON r.Id = ri.RentId
            LEFT JOIN Categories c ON c.Id = i.CategoryId
            WHERE i.IsDeleted = false
                AND i.IsAvailable = true
                AND ST_DWithin(
                    r.location::geography,
                    ST_SetSRID(ST_MakePoint(@Longitude, @Latitude), 4326)::geography,
                    @Radius
                )
            ORDER BY distance_meters ASC
            LIMIT @Limit";

        return await connection.QueryAsync<ItemSearchResult>(query, new
        {
            Latitude = latitude,
            Longitude = longitude,
            Radius = radiusMeters,
            Limit = limit
        });
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        
        var query = @"
            SELECT id, name, description, parent_id as parentid
            FROM categories
            ORDER BY name";

        return await connection.QueryAsync<Category>(query);
    }

    public async Task<Dictionary<string, int>> GetItemsCountByCityAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        
        var query = @"
            SELECT r.city, COUNT(DISTINCT i.id) as count
            FROM items i
            INNER JOIN rent_items ri ON ri.item_id = i.id
            INNER JOIN rents r ON r.id = ri.rent_id
            WHERE i.is_deleted = false AND i.is_available = true
            GROUP BY r.city
            ORDER BY count DESC";

        var result = await connection.QueryAsync<(string City, int Count)>(query);
        return result.ToDictionary(x => x.City, x => x.Count);
    }
}