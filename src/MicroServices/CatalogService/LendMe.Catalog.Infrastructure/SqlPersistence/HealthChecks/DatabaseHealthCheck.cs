using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LendMe.Catalog.Infrastructure.SqlPersistence.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly string _connectionString;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(IConfiguration configuration, ILogger<DatabaseHealthCheck> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            // Проверяем версию PostgreSQL
            using var versionCommand = connection.CreateCommand();
            versionCommand.CommandText = "SELECT version()";
            var version = await versionCommand.ExecuteScalarAsync(cancellationToken);

            // Проверяем наличие PostGIS
            using var postgisCommand = connection.CreateCommand();
            postgisCommand.CommandText = "SELECT PostGIS_version()";
            var postgisVersion = await postgisCommand.ExecuteScalarAsync(cancellationToken);

            // Проверяем размер БД
            using var sizeCommand = connection.CreateCommand();
            sizeCommand.CommandText = @"
                SELECT pg_database_size(current_database()) as size,
                       (SELECT COUNT(*) FROM items WHERE is_deleted = false) as items_count,
                       (SELECT COUNT(*) FROM rents) as locations_count";
            
            using var reader = await sizeCommand.ExecuteReaderAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                var dbSize = reader.GetInt64(0);
                var itemsCount = reader.GetInt64(1);
                var locationsCount = reader.GetInt64(2);

                var data = new Dictionary<string, object>
                {
                    ["PostgreSQL"] = version?.ToString() ?? "Unknown",
                    ["PostGIS"] = postgisVersion?.ToString() ?? "Not installed",
                    ["DatabaseSize"] = $"{dbSize / (1024 * 1024)} MB",
                    ["ItemsCount"] = itemsCount,
                    ["LocationsCount"] = locationsCount
                };

                return HealthCheckResult.Healthy("Database is healthy", data);
            }

            return HealthCheckResult.Healthy("Database is healthy");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            return HealthCheckResult.Unhealthy("Database health check failed", ex);
        }
    }
}

public class PostGISHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    public PostGISHealthCheck(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            // Проверяем работу пространственных функций
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 
                    ST_Distance(
                        ST_SetSRID(ST_MakePoint(76.9293, 43.2567), 4326)::geography,
                        ST_SetSRID(ST_MakePoint(76.9453, 43.2387), 4326)::geography
                    ) as test_distance";
            
            var result = await command.ExecuteScalarAsync(cancellationToken);
            
            if (result != null)
            {
                return HealthCheckResult.Healthy($"PostGIS is working. Test distance: {result:F2} meters");
            }

            return HealthCheckResult.Degraded("PostGIS functions may not be working correctly");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("PostGIS health check failed", ex);
        }
    }
}