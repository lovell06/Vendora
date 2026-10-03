namespace Vendora.Services.Cart.Infrastructure.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}