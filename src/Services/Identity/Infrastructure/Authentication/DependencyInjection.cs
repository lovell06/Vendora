namespace Vendora.Services.Identity.Infrastructure.Authentication;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAuthenticationServices()
        {
            services.AddPasswordHashProvider();
            services.AddAccessTokenProvider();
            services.AddRefreshTokenProvider();
        
            return services;
        }

        public IServiceCollection AddPasswordHashProvider()
        {
            return services.AddScoped<IPasswordHashProvider, AspNetCorePasswordHashProvider>();
        }

        public IServiceCollection AddAccessTokenProvider()
        {
            return services.AddScoped<IAccessTokenProvider, JsonWebTokenProvider>();
        }

        public IServiceCollection AddRefreshTokenProvider()
        {
            return services.AddScoped<IRefreshTokenProvider, RandomNumberTokenProvider>();
        }
    }
}