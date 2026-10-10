using Vendora.BuildingBlocks.Messaging.Abstractions;
using Vendora.BuildingBlocks.Messaging.TypeRegistry;
using Vendora.Services.Identity.Application.IntegrationEvents;

namespace Vendora.Services.Identity.Api;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddJwtBearerAuthentication(IConfiguration configuration)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtOptions = configuration
                    .GetSection(JwtOptions.SectionName)
                    .Get<JwtOptions>() ?? throw new InvalidOperationException("Jwt is not configured.");

                RSAParameters parameters;
                using (var rsa = RSA.Create())
                {
                    rsa.ImportFromPem(File.ReadAllText(jwtOptions.PrivateKeyPath));
                    parameters = rsa.ExportParameters(includePrivateParameters: false);
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(parameters),

                    ValidAlgorithms = [SecurityAlgorithms.RsaSha256]
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerHandler>>();
                        logger.LogWarning(context.Exception, "JWT validation failed.");
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public IServiceCollection AddEventTypeRegistry()
        {
            services.AddSingleton<IEventTypeRegistry>(_ =>
            {
                var registry = new EventTypeRegistry();
                registry.Add("identity.user-registered", typeof(UserRegisteredEvent));

                return registry;
            });
            
            return services;
        }
    }
}