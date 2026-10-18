namespace Vendora.Services.Ordering.Infrastructure.Persistence;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistenceServices(IConfiguration configuration)
        {
            services.AddDbContextConnection(configuration);
            services.AddUnitOfWorkServices();

            return services;
        }

        public IServiceCollection AddDbContextConnection(IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var conn = configuration.GetConnectionString("Postgres")
                           ?? throw new InvalidOperationException("Postgres connection string is not configured.");

                options.UseNpgsql(conn);
            });

            return services;
        }

        public IServiceCollection AddUnitOfWorkServices()
        {
            return services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
        }
    }
}
