namespace Vendora.Services.Identity.Infrastructure.Email;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddEmailServices()
        {
            services.AddEmailSender();
            services.AddEmailVerificationTokenProvider();
        
            return services;
        }

        public IServiceCollection AddEmailSender()
        {
            return services.AddScoped<IEmailSender, SmtpEmailSender>();
        }

        public IServiceCollection AddEmailVerificationTokenProvider()
        {
            return services.AddScoped<IEmailVerificationTokenProvider, RedisEmailVerificationTokenProvider>();
        }
    }
}