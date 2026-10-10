namespace Vendora.Services.Identity.Infrastructure.Persistence;

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
                                   ?? throw new InvalidOperationException("Postgres connection is not configured.");

            services.AddDbContext<ApplicationDbContext>((provider, builder) =>
            {
                builder.UseNpgsql(connectionString);
                builder.UseSeeding((context, _) =>
                {
                    AdminAccountSeeder.Seed(context, provider, configuration);
                });

                builder.UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    await AdminAccountSeeder.SeedAsync(context, provider, configuration, cancellationToken);
                });
            });

            return services;
        }

        public IServiceCollection AddUnitOfWorkServices()
        {
            return services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
        }
    }
}