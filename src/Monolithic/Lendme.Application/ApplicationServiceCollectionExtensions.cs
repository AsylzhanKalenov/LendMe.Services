using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Lendme.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static void ConfigureApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}