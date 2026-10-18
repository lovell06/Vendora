namespace Vendora.Services.Ordering.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationHandlers()
        {        
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            return services;
        }

        public IServiceCollection AddTimeProvider()
        {
            return services.AddSingleton(TimeProvider.System);
        }
    }
}
