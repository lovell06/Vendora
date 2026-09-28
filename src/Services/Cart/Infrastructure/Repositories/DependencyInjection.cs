using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Cart.Domain.Carts;

namespace Vendora.Services.Cart.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICartRepository, PostgresCartRepository>();
        
        return services;
    }
}