namespace Vendora.Services.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRedisConnection(configuration);
        services.AddPersistence(configuration);
        services.AddInfrastructureAuthentication();
        services.AddInfrastructureEmail();
        services.AddInfrastructureOptions();
        services.AddRepositoryServices();
        services.AddClients(configuration);

        return services;
    }
}
