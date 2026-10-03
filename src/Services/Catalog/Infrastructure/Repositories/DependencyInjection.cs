namespace Vendora.Services.Catalog.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
        services.AddScoped<IProductRepository, EfCoreProductRepository>();
        
        return services;
    }
}