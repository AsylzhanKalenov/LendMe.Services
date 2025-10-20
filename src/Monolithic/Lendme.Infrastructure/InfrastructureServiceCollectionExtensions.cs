using Lendme.Application.Notification.Interface;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using Lendme.Infrastructure.Services.KafkaService;
using Lendme.Infrastructure.SqlPersistence;
using Lendme.Infrastructure.SqlPersistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lendme.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Kafka Services
        services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
        
        services.AddPostgreSqlDb(configuration);
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis"); // "redis:6379"
            options.InstanceName = "lendme:"; // необязательно
        });
        
        // Регистрация репозиториев
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.Decorate<IBookingRepository, BookingRepositoryCached>();
        // services.AddScoped<IBookingRepository>(sp =>
        // {
        //     var inner = sp.GetRequiredService<BookingRepository>();          // базовая реализация
        //     var cache = sp.GetRequiredService<IDistributedCache>();            // Redis-кэш
        //     return new BookingRepositoryCached(inner, cache);                  // декоратор
        // });
        
        return services;
    }

    static IServiceCollection AddPostgreSqlDb(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Entity Framework
        services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}