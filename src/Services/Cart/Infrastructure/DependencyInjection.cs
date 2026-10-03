namespace Vendora.Services.Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationServices();
        services.AddPersistence(configuration);
        services.AddRepositories();
        services.AddQueryServices();
        
        return services;
    }
}