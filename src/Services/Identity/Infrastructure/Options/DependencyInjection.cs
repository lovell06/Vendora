using Microsoft.Extensions.DependencyInjection;

namespace Vendora.Services.Identity.Infrastructure.Options;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureOptions(this IServiceCollection services)
    {
        services.AddOptions<SmtpOptions>()
            .BindConfiguration(SmtpOptions.SectionName)
            .ValidateOnStart();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateOnStart();

        return services;
    }
}