namespace Vendora.Services.Catalog.Infrastructure.Authentication;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCurrentUserService()
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();
            return services;
        }
    }
}