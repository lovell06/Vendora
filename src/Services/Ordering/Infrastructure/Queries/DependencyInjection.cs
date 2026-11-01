namespace Vendora.Services.Ordering.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<IGetOrderQueryService, EfCoreGetOrderQueryService>();
            services.AddScoped<IListOrdersQueryService, EfCoreListOrdersQueryService>();
            return services;
        }
    }
}