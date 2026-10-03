namespace Vendora.Services.Identity.Infrastructure.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHashProvider, AspNetCorePasswordHashProvider>();

        services.AddScoped<IAccessTokenProvider, JsonWebTokenProvider>();

        services.AddScoped<IRefreshTokenProvider, RandomNumberTokenProvider>();
        
        return services;
    }
}