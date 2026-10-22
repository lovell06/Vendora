namespace Vendora.Services.Ordering.Infrastructure.Repositories;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRepositoryServices()
        {
            services.AddScoped<IOrderRepository, EfCoreOrderRepository>();
            services.AddScoped<IOutboxMessageRepository, EfCoreOutboxMessageRepository>();
            return services;
        }
    }
}