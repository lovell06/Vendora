using Vendora.Services.Inventory.Application.InventoryItems.CheckAvailability;
using Vendora.Services.Inventory.Application.InventoryItems.Get;
using Vendora.Services.Inventory.Application.InventoryItems.List;

namespace Vendora.Services.Inventory.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<IGetInventoryItemQueryService, EfCoreGetInventoryItemQueryService>();
            services.AddScoped<IListInventoryItemsQueryService, EfCoreListInventoryItemsQueryService>();
            services.AddScoped<ICheckAvailabilityQueryService, EfCoreCheckAvailabilityQueryService>();
        
            return services;
        }
    }
}