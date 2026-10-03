namespace Vendora.Services.Cart.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICartRepository, EfCoreCartRepository>();
        
        return services;
    }
}