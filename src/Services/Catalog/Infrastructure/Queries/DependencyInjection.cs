using Vendora.Services.Catalog.Application.Categories.Get;
using Vendora.Services.Catalog.Application.Categories.List;
using Vendora.Services.Catalog.Application.Products.Get;
using Vendora.Services.Catalog.Application.Products.List;

namespace Vendora.Services.Catalog.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<IListProductsQueryService, EfCoreListProductsQueryService>();
            services.AddScoped<IGetProductQueryService, EfCoreGetProductQueryService>();
            services.AddScoped<IListCategoryQueryService, EfCoreListCategoriesQuerySerivce>();
            services.AddScoped<IGetCategoryQueryService, EfCoreGetCategoryQueryService>();
        
            return services;
        }
    }
}