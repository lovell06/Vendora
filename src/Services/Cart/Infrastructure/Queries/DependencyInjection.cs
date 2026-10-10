namespace Vendora.Services.Cart.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<IListItemsQueryService, EfCoreListItemQueryService>();
        
            return services;
        }
    }
}