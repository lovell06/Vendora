namespace Vendora.Services.Identity.Infrastructure.Redis;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRedisConnection(IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var connString = configuration
                                     .GetConnectionString("Redis") 
                                 ?? throw new InvalidOperationException("Redis connection is not configured.");

                return ConnectionMultiplexer.Connect(connString);
            });

            return services;
        }
    }
}