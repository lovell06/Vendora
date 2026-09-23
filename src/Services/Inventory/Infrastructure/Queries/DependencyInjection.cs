using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Inventory.Application.InventoryItems.Get;
using Vendora.Services.Inventory.Application.InventoryItems.List;

namespace Vendora.Services.Inventory.Infrastructure.Queries;

public static class DependencyInjection
{
    public static IServiceCollection AddQueryServices(this IServiceCollection services)
    {
        services.AddScoped<IGetInventoryItemQueryService, PostgresGetInventoryItemQueryService>();
        services.AddScoped<IListInventoryItemsQueryService, PostgresListInventoryItemsQueryService>();
        
        return services;
    }
}