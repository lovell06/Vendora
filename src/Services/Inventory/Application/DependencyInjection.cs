namespace Vendora.Services.Inventory.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationHandlers()
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            return services;
        }

        public IServiceCollection AddTimeProvider()
        {
            return services.AddSingleton(TimeProvider.System);
        }
    }
}