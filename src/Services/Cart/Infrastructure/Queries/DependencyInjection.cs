namespace Vendora.Services.Cart.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<ListCartItems.IQueryService, EfCoreListCartItemQueryService>();
            services.AddScoped<GetSelectedCartItems.IQueryService, EfCoreGetSelectedCartItemsQueryService>();
        
            return services;
        }
    }
}