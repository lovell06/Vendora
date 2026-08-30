using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Identity.Application.Abstractions.Email;

namespace Vendora.Services.Identity.Infrastructure.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEmail(this IServiceCollection services)
    {
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IEmailVerificationTokenProvider, RedisEmailVerificationTokenProvider>();
        
        return services;
    }
}