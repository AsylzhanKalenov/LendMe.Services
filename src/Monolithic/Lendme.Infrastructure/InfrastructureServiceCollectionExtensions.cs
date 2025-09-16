using Lendme.Core.Interfaces;
using Lendme.Core.Interfaces.Repositories;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using Lendme.Core.Interfaces.Services.ChatServices;
using Lendme.Infrastructure.Implementations;
using Lendme.Infrastructure.MongoPersistence;
using Lendme.Infrastructure.Services;
using Lendme.Infrastructure.SqlPersistence;
using Lendme.Infrastructure.SqlPersistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Lendme.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Chat services
        services.AddScoped<IChatService, MongoDbChatService>();
        services.AddScoped<IChatNotificationService, ChatNotificationService>();
        
        services.AddMongoDb(configuration);
        services.AddPostgreSqlDb(configuration);
        
        // Регистрация репозиториев
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        
        return services;
    }

    static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection("MongoDB"));

        
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        // Регистрация базы данных
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });
        
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