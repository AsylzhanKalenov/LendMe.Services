using LendMe.Shared.Application;
using LendMe.Shared.Infrastructure;

namespace LendMe.Shared.Web.Feature.AutoMapper;

public static class AutoMapperServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        // Pass assemblies which contains mapping profiles.
        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddAutoMapper(typeof(InfrastructureServiceCollectionExtensions).Assembly);
        services.AddAutoMapper(typeof(ApplicationServiceCollectionExtensions).Assembly);
        return services;
    }
}