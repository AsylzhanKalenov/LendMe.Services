using System.Text;
using Dapper;
using LendMe.Catalog.Core.Dto;
using LendMe.Catalog.Core.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace LendMe.Catalog.Infrastructure.SqlPersistence.Repositories;

public class RentSearchRepository : IRentSearchRepository
{
    private readonly string _connectionString;

    public RentSearchRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<RentSearchResult>> GetNearbyRentsAsync(double latitude, double longitude, int radiusMeters, int limit = 10)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        
        var query = @"
            SELECT 
                r.id,
                r.title,
                r.description,
                r.min_price as MinPrice,
                r.max_price as MaxPrice,
                r.longitude,
                r.latitude,
                r.address,
                r.city,
                r.district,
                r.radius_meters as RadiusMeters,
                r.created_at as CreatedAt,
                ST_Distance(
                    r.location::geography,
                    ST_SetSRID(ST_MakePoint(@Longitude, @Latitude), 4326)::geography
                ) as distance_meters
            FROM rents r
            WHERE ST_DWithin(
                r.location::geography,
                ST_SetSRID(ST_MakePoint(@Longitude, @Latitude), 4326)::geography,
                @Radius
            )
            ORDER BY distance_meters ASC
            LIMIT @Limit";

        return await connection.QueryAsync<RentSearchResult>(query, new
        {
            Latitude = latitude,
            Longitude = longitude,
            Radius = radiusMeters,
            Limit = limit
        });
    }
    
    public async Task<PagedResult<RentSearchResult>> SearchRentsAsync(SearchRentsRequest request)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        
        var sqlBuilder = new StringBuilder();
        var parameters = new DynamicParameters();
        
        // Основной запрос с JOIN-ами
        sqlBuilder.Append(@"
            WITH search_results AS (
                SELECT DISTINCT
                    r.id,
                    r.title,
                    r.description,
                    r.min_price as MinPrice,
                    r.max_price as MaxPrice,
                    r.longitude,
                    r.latitude,
                    r.address,
                    r.city,
                    r.district,
                    r.radius_meters as RadiusMeters,
                    r.created_at as CreatedAt");

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
            FROM rents r");

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
        var items = await connection.QueryAsync<RentSearchResult>(sqlBuilder.ToString(), parameters);

        // Получаем общее количество записей для пагинации
        var countQuery = BuildCountQuery(request);
        var totalCount = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

        return new PagedResult<RentSearchResult>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
    
    private void BuildWhereClause(StringBuilder sqlBuilder, DynamicParameters parameters, SearchRentsRequest request)
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
            sqlBuilder.Append(" AND r.category_id = @CategoryId");
            parameters.Add("CategoryId", request.CategoryId.Value);
        }

        // Фильтр по цене
        if (request.MinPrice.HasValue)
        {
            sqlBuilder.Append(" AND r.min_price >= @MinPrice");
            parameters.Add("MinPrice", request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            sqlBuilder.Append(" AND r.max_price <= @MaxPrice");
            parameters.Add("MaxPrice", request.MaxPrice.Value);
        }

        // Поиск по тексту
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            sqlBuilder.Append(" AND r.title ILIKE @SearchText");
            parameters.Add("SearchText", $"%{request.SearchText}%");
        }

        // TODO: Consider rent status
        // Фильтр по статусу
        // if (request.Status.HasValue)
        // {
        //     sqlBuilder.Append(" AND i.status = @Status");
        //     parameters.Add("Status", (int)request.Status.Value);
        // }

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

    private void AddSorting(StringBuilder sqlBuilder, SearchRentsRequest request)
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
            case SortBy.MinPriceAsc:
                sqlBuilder.Append("min_price ASC");
                break;
            case SortBy.MinPriceDesc:
                sqlBuilder.Append("min_price DESC");
                break;
            case SortBy.MaxPriceAsc:
                sqlBuilder.Append("max_price ASC");
                break;
            case SortBy.MaxPriceDesc:
                sqlBuilder.Append("max_price DESC");
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

    private string BuildCountQuery(SearchRentsRequest request)
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
}