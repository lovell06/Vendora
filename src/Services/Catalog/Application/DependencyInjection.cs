using Microsoft.Extensions.DependencyInjection;

namespace Vendora.Services.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(
                typeof(DependencyInjection).Assembly);
        });

        return services;
    }
}