namespace Vendora.Services.Cart.Infrastructure.Persistence;

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
                                   ?? throw new InvalidOperationException(
                                       "PostgreSQL connection string is not configured.");
        
            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseNpgsql(connectionString);
            });

            return services;
        }

        public IServiceCollection AddUnitOfWorkServices()
        {
            return services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
        }
    }
}