using Microsoft.Extensions.DependencyInjection;

namespace Vendora.Services.Inventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}