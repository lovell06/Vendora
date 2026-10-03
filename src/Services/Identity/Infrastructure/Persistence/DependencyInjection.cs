namespace Vendora.Services.Identity.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
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

        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
        
        return services;
    }
}