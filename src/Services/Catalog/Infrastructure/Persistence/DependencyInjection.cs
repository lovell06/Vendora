namespace Vendora.Services.Catalog.Infrastructure.Persistence;

internal static class DependencyInjection
{
    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
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

        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
        return services;
    }
}