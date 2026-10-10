namespace Vendora.Services.Identity.Application;

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
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            return services;
        }
    }
}