namespace Vendora.Services.Inventory.Infrastructure.Persistence;

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
            var connectionString = configuration.GetConnectionString("Postgres")
                                   ?? throw new InvalidOperationException("Postgres connection string is not configured.");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            return services;
        }

        public IServiceCollection AddUnitOfWorkServices()
        {
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            return services;
        }
    }
}