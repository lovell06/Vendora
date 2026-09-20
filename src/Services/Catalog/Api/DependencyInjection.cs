using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Vendora.Services.Catalog.Api.Constants;

namespace Vendora.Services.Catalog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
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
            var jwt = configuration.GetRequiredSection("Jwt");

            var publicKeyPem = jwt["PublicKeyPem"]
                               ?? throw new InvalidOperationException("Missing Jwt:PublicKeyPem.");

            RSAParameters rsaParameters;
            using (var rsa = RSA.Create())
            {
                rsa.ImportFromPem(publicKeyPem);
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
}