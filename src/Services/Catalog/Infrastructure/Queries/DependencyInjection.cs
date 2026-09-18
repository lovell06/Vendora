using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Catalog.Application.Categories.List;
using Vendora.Services.Catalog.Application.Products.Get;
using Vendora.Services.Catalog.Application.Products.List;
using Vendora.Services.Catalog.Infrastructure.Queries.Categories;
using Vendora.Services.Catalog.Infrastructure.Queries.Products;

namespace Vendora.Services.Catalog.Infrastructure.Queries;

public static class DependencyInjection
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IListProductsQueryService, PostgresListProductsQueryService>();
        services.AddScoped<IGetProductQueryService, PostgresGetProductQueryService>();
        services.AddScoped<IListCategoryQueryService, PostgresListCategoriesQuerySerivce>();
        
        return services;
    }
}