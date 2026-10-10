namespace Vendora.Services.Catalog.Infrastructure.Persistence;

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
                options.UseNpgsql(configuration.GetConnectionString("Postgres"));

                options.UseSeeding((context, _) =>
                {
                    CategoryDataSeeder.Seed(context, configuration);
                    ProductDataSeeder.Seed(context, configuration);
                });

                options.UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    await CategoryDataSeeder.SeedAsync(context, configuration, cancellationToken);
                    await ProductDataSeeder.SeedAsync(context, configuration, cancellationToken);
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