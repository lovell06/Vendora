namespace Vendora.Services.Identity.Infrastructure.Options;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureOptions()
        {
            services.AddSmtpOptions();
            services.AddJwtOptions();

            return services;
        }

        public IServiceCollection AddSmtpOptions()
        {
            services.AddOptions<SmtpOptions>()
                .BindConfiguration(SmtpOptions.SectionName)
                .ValidateOnStart();

            return services;
        }

        public IServiceCollection AddWebOptions()
        {
            services.AddOptions<WebOptions>()
                .BindConfiguration(WebOptions.SectionName)
                .Validate(
                    options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var uri)
                               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps),
                    "Web:BaseUrl must be an absolute http(s) URL.")
                .ValidateOnStart();

            return services;
        }

        public IServiceCollection AddJwtOptions()
        {
            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateOnStart();

            return services;
        }
    }
}