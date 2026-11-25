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
            services.AddCurrentUserService();
        
            return services;
        }

        public IServiceCollection AddCurrentUserService()
        {
            services.AddHttpContextAccessor();
            return services.AddScoped<ICurrentUser, CurrentUser>();
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