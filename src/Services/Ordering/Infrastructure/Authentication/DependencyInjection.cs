namespace Vendora.Services.Ordering.Infrastructure.Authentication;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCurrentUserService()
        {
            services.AddHttpContextAccessor();
            return services.AddScoped<ICurrentUser, CurrentUser>();
        }
    }
}