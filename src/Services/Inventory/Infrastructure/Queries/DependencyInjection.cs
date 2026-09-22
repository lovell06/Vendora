using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Inventory.Application.InventoryItems.Get;

namespace Vendora.Services.Inventory.Infrastructure.Queries;

public static class DependencyInjection
{
    public static IServiceCollection AddQueryServices(this IServiceCollection services)
    {
        services.AddScoped<IGetInventoryItemQueryService, PostgresGetInventoryItemQueryService>();

        return services;
    }
}