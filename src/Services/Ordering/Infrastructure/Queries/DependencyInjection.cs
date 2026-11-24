namespace Vendora.Services.Ordering.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<GetOrder.IQueryService, EfCoreGetOrderQueryService>();
            services.AddScoped<ListOrders.IQueryService, EfCoreListOrdersQueryService>();
            return services;
        }
    }
}