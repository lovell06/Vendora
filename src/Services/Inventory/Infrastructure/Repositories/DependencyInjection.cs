namespace Vendora.Services.Inventory.Infrastructure.Repositories;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRepositoryServices()
        {
            services.AddScoped<IInventoryItemRepository, EfCoreInventoryItemRepository>();
        
            return services;
        }
    }
}