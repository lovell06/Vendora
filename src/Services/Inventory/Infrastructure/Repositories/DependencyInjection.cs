using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Inventory.Domain.InventoryItems;

namespace Vendora.Services.Inventory.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IInventoryItemRepository, PostgresInventoryItemRepository>();
        
        return services;
    }
}