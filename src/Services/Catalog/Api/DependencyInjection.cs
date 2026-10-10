namespace Vendora.Services.Catalog.Api;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddJwtBearerAuthentication(IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
        
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
                options.SaveToken = true;
            
                var jwt = configuration.GetRequiredSection("Jwt");

                var publicKeyPath = jwt["PublicKeyPath"]
                                    ?? throw new InvalidOperationException("Missing Jwt:PublicKeyPath.");

                RSAParameters rsaParameters;
                using (var rsa = RSA.Create())
                {
                    rsa.ImportFromPem(File.ReadAllText(publicKeyPath));
                    rsaParameters = rsa.ExportParameters(includePrivateParameters: false);
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt["Issuer"] ?? throw new InvalidOperationException("Missing Jwt:Issuer."),

                    ValidateAudience = true,
                    ValidAudience = jwt["Audience"] ?? throw new InvalidOperationException("Missing Jwt:Audience."),

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(rsaParameters),

                    ValidAlgorithms = [SecurityAlgorithms.RsaSha256]
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerHandler>>();
                        logger.LogWarning(context.Exception, "JWT validation failed.");
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerHandler>>();
                        logger.LogWarning("User forbidden.");
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.ManageProducts, poilcy =>
                {
                    poilcy.RequireAuthenticatedUser();
                    poilcy.RequireRole(RoleNames.Admin);
                });

                options.AddPolicy(AuthorizationPolicies.ManageCategories, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(RoleNames.Admin);
                });
            });

            return services;
        }

        public IServiceCollection AddEventTypeRegistry()
        {
            services.AddSingleton<IEventTypeRegistry>(_ =>
            {
                var registry = new EventTypeRegistry();
                registry.Add("catalog.product-created", typeof(ProductCreatedEvent));

                return registry;
            });
            return services;
        }
    }
}