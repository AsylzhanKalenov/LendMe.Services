using Lendme.Infrastructure.MongoPersistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lendme.Infrastructure;

public static class InfrastructureApplicationBuilderExtensions
{
    public static IApplicationBuilder EnsureMongoDbIndexes(this IServiceCollection services, IApplicationBuilder app)
    {
        // Регистрируем стратегию генерации ID
        
        
        return app;
    }
}