using Vendora.Services.Catalog.Application.Categories.Get;
using Vendora.Services.Catalog.Application.Categories.List;
using Vendora.Services.Catalog.Application.Products.Get;
using Vendora.Services.Catalog.Application.Products.List;

namespace Vendora.Services.Catalog.Infrastructure.Queries;

public static class DependencyInjection
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IListProductsQueryService, EfCoreListProductsQueryService>();
        services.AddScoped<IGetProductQueryService, EfCoreGetProductQueryService>();
        services.AddScoped<IListCategoryQueryService, EfCoreListCategoriesQuerySerivce>();
        services.AddScoped<IGetCategoryQueryService, EfCoreGetCategoryQueryService>();
        
        return services;
    }
}