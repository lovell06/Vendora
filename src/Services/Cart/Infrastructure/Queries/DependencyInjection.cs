using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Cart.Application.Carts.ListItems;

namespace Vendora.Services.Cart.Infrastructure.Queries;

public static class DependencyInjection
{
    public static IServiceCollection AddQueryServices(this IServiceCollection services)
    {
        services.AddScoped<IListItemsQueryService, EfCoreListItemQueryService>();
        
        return services;
    }
}