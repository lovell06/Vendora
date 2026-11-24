namespace Vendora.Services.Catalog.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<ListProducts.IQueryService, EfCoreListProductsQueryService>();
            services.AddScoped<GetProduct.IQueryService, EfCoreGetProductQueryService>();
            services.AddScoped<ListCategories.IQueryService, EfCoreListCategoriesQuerySerivce>();
            services.AddScoped<GetCategory.IQueryService, EfCoreGetCategoryQueryService>();
            services.AddScoped<ListProductsByIds.IQueryService, EfCoreListProductsByIdsQueryService>();
            services.AddScoped<GetProductVariant.IQueryService, EfCoreGetVariantQueryService>();
        
            return services;
        }
    }
}