namespace Vendora.Services.Catalog.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddTimeProvider()
        {
            return services.AddSingleton(TimeProvider.System);
        }

        public IServiceCollection AddApplicationHandlers()
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(
                    typeof(DependencyInjection).Assembly);
            });

            return services;
        }
    }
}