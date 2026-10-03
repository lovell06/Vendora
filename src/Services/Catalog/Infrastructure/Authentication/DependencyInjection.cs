namespace Vendora.Services.Catalog.Infrastructure.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        
        return services;
    }
}