using System.Data;
using LendMe.Catalog.Core.Interfaces.Repository;
using LendMe.Catalog.Core.Interfaces.Services;
using LendMe.Catalog.Infrastructure.Services;
using LendMe.Catalog.Infrastructure.SqlPersistence.Context;
using LendMe.Catalog.Infrastructure.SqlPersistence.HealthChecks;
using LendMe.Catalog.Infrastructure.SqlPersistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace LendMe.Catalog.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgreSqlDb(configuration);
        services.AddMemoryCache();
        return services;
    }

    static IServiceCollection AddPostgreSqlDb(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Entity Framework
        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptions =>
            {
                npgsqlOptions.UseNetTopologySuite();
                //npgsqlOptions.MigrationsAssembly("LendMe.Catalog.Infrastructure");
            }).UseSnakeCaseNamingConvention();
        });
        
        // Add Dapper
        services.AddScoped<IDbConnection>(sp => 
            new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IItemSearchRepository, ItemSearchRepository>();
        services.AddScoped<IItemSearchService, ItemSearchService>();
        services.AddScoped<IRentRepository, RentRepository>();
        
        services.AddHealthChecks()
            .AddTypeActivatedCheck<DatabaseHealthCheck>("database")
            .AddTypeActivatedCheck<PostGISHealthCheck>("postgis")
            .AddDbContextCheck<ApplicationDbContext>("ef_core_context");

        return services;
    }
}