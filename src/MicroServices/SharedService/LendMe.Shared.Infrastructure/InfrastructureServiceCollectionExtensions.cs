using LendMe.Shared.Application.Interfaces.ChatServices;
using LendMe.Shared.Application.Interfaces.NotificationServices;
using LendMe.Shared.Core.Repositories;
using LendMe.Shared.Infrastructure.MongoPersistence;
using LendMe.Shared.Infrastructure.MongoPersistence.Implementations;
using LendMe.Shared.Infrastructure.Services;
using LendMe.Shared.Infrastructure.Services.KafkaService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LendMe.Shared.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Kafka Services
        services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
        services.AddHostedService<KafkaConsumerService>();
        
        // Chat services
        services.AddScoped<IChatService, MongoDbChatService>();
        services.AddScoped<IChatNotificationService, ChatNotificationService>();
        
        services.AddMongoDb(configuration);
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis"); // "redis:6379"
            options.InstanceName = "lendme:"; // необязательно
        });
        
        services.AddScoped<IReviewRepository, ReviewRepository>();
        
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
}