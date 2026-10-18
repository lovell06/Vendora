namespace Vendora.Services.Catalog.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<IListProductsQueryService, EfCoreListProductsQueryService>();
            services.AddScoped<IGetProductQueryService, EfCoreGetProductQueryService>();
            services.AddScoped<IListCategoriesQueryService, EfCoreListCategoriesQuerySerivce>();
            services.AddScoped<IGetCategoryQueryService, EfCoreGetCategoryQueryService>();
            services.AddScoped<IListProductsByIdsQueryService, EfCoreListProductsByIdsQueryService>();
        
            return services;
        }
    }
}