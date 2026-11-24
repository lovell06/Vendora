namespace Vendora.Services.Inventory.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<GetInventoryItem.IQueryService, EfCoreGetInventoryItemQueryService>();
            services.AddScoped<ListInventoryItems.IQueryService, EfCoreListInventoryItemsQueryService>();
            services.AddScoped<CheckInventoryAvailability.IQueryService, EfCoreCheckAvailabilityQueryService>();
        
            return services;
        }
    }
}