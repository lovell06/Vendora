using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Catalog.Domain.Categories;
using Vendora.Services.Catalog.Domain.Products;

namespace Vendora.Services.Catalog.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, PostgresCategoryRepository>();
        services.AddScoped<IProductRepository, PostgresProductRepository>();
        
        return services;
    }
}