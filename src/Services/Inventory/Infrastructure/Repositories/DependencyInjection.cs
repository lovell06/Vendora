namespace Vendora.Services.Inventory.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IInventoryItemRepository, EfCoreInventoryItemRepository>();
        
        return services;
    }
}