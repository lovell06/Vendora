namespace Vendora.Services.Catalog.Infrastructure.Repositories;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRepositoryServices()
        {
            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<IOutboxRepository, EfCoreOutboxRepository>();
        
            return services;
        }
    }
}