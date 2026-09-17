using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Catalog.Application.Products.List;
using Vendora.Services.Catalog.Infrastructure.Queries.Products;

namespace Vendora.Services.Catalog.Infrastructure.Queries;

public static class DependencyInjection
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IListProductsQueryService, PostgresListProductsQueryService>();
        
        return services;
    }
}