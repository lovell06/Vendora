using Microsoft.Extensions.DependencyInjection;

namespace Vendora.Services.Identity.Infrastructure.Options;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureOptions(this IServiceCollection services)
    {
        services.AddOptions<SmtpOptions>()
            .BindConfiguration(SmtpOptions.SectionName)
            .ValidateOnStart();

        return services;
    }
}