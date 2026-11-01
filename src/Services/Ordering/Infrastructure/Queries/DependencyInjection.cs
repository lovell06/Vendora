using Vendora.Services.Ordering.Application.Orders.Get;

namespace Vendora.Services.Ordering.Infrastructure.Queries;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueryServices()
        {
            services.AddScoped<IQueryService, EfCoreGetOrderQueryService>();
            return services;
        }
    }
}